ALTER PROCEDURE [dbo].[CreateCatalogsForPhase]
        @myPhaseID int
AS 
Begin
	DECLARE @BookID int, 
			@DepartmentID int, 
			@BookCount int,
			@BookDiscountID int,
			@BookDiscountFrom int,
			@BookDiscountTo int,
			@YearDiscountID int,
			@YearDiscountPercentage int,
			@BookpRiceID int,
			@PhaseID int,
			@SupplierID int,
			@Percentage decimal(5,2),
			@BookFirstRegistrationYear int,
			@BookUnitPrice decimal(5,2), 
			@BookDiscountPercentage decimal(5,2),

			@TotalBookCount int,
			@PreviousBookID int,
			@PreviousDepartmentID int,
			@SubBookCountLow int,
			@SubBookCountHigh int, 
			@CatalogAmount decimal(8,2), 
			@PreviousDiscountID int,
			@PreviousDiscountFrom int,
			@PreviousDiscountTo int,
			@SelectedPhaseYear int,
			@PerYearDiscountPercentage decimal(5,2),
			@PreviousBookDiscountPercentage decimal(5,2)



	SET @TotalBookCount = 0 
	SET	@PreviousBookID = 0
	Set @PreviousDiscountTo = 0
	Set @PreviousDiscountFrom = 0
	SET	@PreviousDepartmentID = 0
	SET @SubBookCountLow = 0
	SET @SubBookCountHigh = 0
	SET @CatalogAmount = 0

	Select @YearDiscountID = IsNull(ID,0), @YearDiscountPercentage = ISNULL(DiscountPercentage,1) from Discount where PhaseID = @myPhaseID and BookID is null
	Select @SelectedPhaseYear = Year from Phase where ID = @myPhaseID

	IF EXISTS(SELECT name FROM sys.sysobjects WHERE Name = N'tempReceipt' AND xtype = N'U')
	BEGIN 
		DROP table tempReceipt
	END

	Select * into tempReceipt 
	from Receipt where PhaseID = @myPhaseID

	DECLARE ReceiptsCursor CURSOR FOR   
	Select 
		tr.BookID, 
		tr.PhaseID,
		DepartmentID, 
		Sum(BookCount) as BookCount, 
		bp.ID,
		bs.SupplierID,
		bs.Percentage,
		b.FirstRegistrationYear,
		bp.Price
	from tempReceipt tr
	inner join Phase p on p.ID = tr.PhaseID
	left join BookPrice bp on bp.BookID = tr.BookID and bp.Status = 1 and bp.Year = p.Year
	inner join BookSupplier bs on bs.BookID = tr.BookID and bs.Year = p.Year and bs.IsActive = 1
	inner join Book b on b.ID = tr.BookID 
	where tr.State = 1 
	Group By tr.PhaseID, tr.BookID, DepartmentID, Bp.ID, Bs.SupplierID, Bs.Percentage, b.FirstRegistrationYear, bp.Price
	Order by tr.BookID, DepartmentID
  
	OPEN ReceiptsCursor  
  	BEGIN TRY
		BEGIN TRANSACTION

	FETCH NEXT FROM ReceiptsCursor   
	INTO @BookID, @PhaseID, @DepartmentID, @BookCount, @BookPriceID, @SupplierID, @Percentage, @BookFirstRegistrationYear, @BookUnitPrice
  
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	-- For new condition of discount according to FirstRegistrationPhase
	-- Reduce price for 3% for every year after 5 years of FirstRegistration until 15% is reached
		if (ISNULL(@BookFirstRegistrationYear, 0) > 0) AND ((@SelectedPhaseYear - @BookFirstRegistrationYear) > 5)
		Begin
			Select @PerYearDiscountPercentage = 1 - IIF(IIF(@SelectedPhaseYear-@BookFirstRegistrationYear - 5 > 0, @SelectedPhaseYear-@BookFirstRegistrationYear - 5, 5) < 5, IIF(@SelectedPhaseYear-@BookFirstRegistrationYear - 5 > 0, @SelectedPhaseYear-@BookFirstRegistrationYear - 5, 5), 5) * 0.03
		end
		ELSE
		Begin
			Select @PerYearDiscountPercentage = 1
		End
		if @PreviousBookID = @BookID
		BEGIN
			SET @TotalBookCount += @BookCount
			if (@TotalBookCount > @PreviousDiscountTo) AND (@PreviousDiscountTo <> 0)
			Begin
				Select @BookDiscountID = ISNULL(ID,0), @BookDiscountFrom = BookCountFrom, @BookDiscountTo = BookCountTo, @BookDiscountPercentage = ISNULL(DiscountPercentage, -1)
					from Discount where BookID = @BookID and PhaseID = @myPhaseID and @TotalBookCount >= BookCountFrom and @TotalBookCount <= BookCountTo

				if @TotalBookCount - @BookCount < @BookDiscountFrom
				Begin
					SET @SubBookCountLow = @BookCount -  (@TotalBookCount - @BookDiscountFrom) - 1
					SET @SubBookCountHigh = @BookCount - @SubBookCountLow 

					if @SubBookCountLow > 0
					begin
						SET @CatalogAmount = @SubBookCountLow * @BookUnitPrice * IIF(@PreviousBookDiscountPercentage > 0, @PreviousBookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage 
						
						Insert Into Catalog (CatalogType, PhaseID,OriginalPhaseID ,BookID, SupplierID, DepartmentID, DiscountID, BookCount, Percentage, BookPriceID, Amount, State, Status, 
						CreatedAt, CreatedBy)
						Values (0, @PhaseID, @PhaseID, @BookID, @SupplierId, @DepartmentID, @PreviousDiscountID, 
						@SubBookCountLow, @Percentage, @BookpRiceID, @CatalogAmount, 0 , 1, GETDATE(), 'sysadmin')
					end

					SET @CatalogAmount = @SubBookCountHigh * @BookUnitPrice * IIF(@BookDiscountPercentage > 0, @BookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage 
					
					Insert Into Catalog (CatalogType, PhaseID, OriginalPhaseID, BookID, SupplierID, DepartmentID, DiscountID, BookCount, Percentage, BookPriceID, Amount, State, Status, 
					CreatedAt, CreatedBy)
					Values (0, @PhaseID, @PhaseID, @BookID, @SupplierId, @DepartmentID, @BookDiscountID, 
					@SubBookCountHigh, @Percentage, @BookpRiceID, @CatalogAmount, 0 , 1, GETDATE(), 'sysadmin')
				End
				Else
				Begin
						SET @CatalogAmount = @BookCount * @BookUnitPrice * IIF(@BookDiscountPercentage > 0, @BookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage 

						Insert Into Catalog (CatalogType, PhaseID, OriginalPhaseID, BookID, SupplierID, DepartmentID, DiscountID, BookCount, Percentage, BookPriceID, Amount, State, Status, 
						CreatedAt, CreatedBy)
						Values (0, @PhaseID, @PhaseID, @BookID, @SupplierId, @DepartmentID, @BookDiscountID, 
						@BookCount, @Percentage, @BookpRiceID, @CatalogAmount, 0 , 1, GETDATE(), 'sysadmin')
				End

			SET @PreviousDiscountID = @BookDiscountID 
			SET @PreviousDiscountFrom = @BookDiscountFrom
			SET @PreviousDiscountTo = @BookDiscountTo
			SET @PreviousBookDiscountPercentage = @BookDiscountPercentage

			End
			Else
			Begin
					SET @CatalogAmount = @BookCount * @BookUnitPrice * IIF(@BookDiscountPercentage > 0, @BookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage 

					Insert Into Catalog (CatalogType, PhaseID, OriginalPhaseID, BookID, SupplierID, DepartmentID, DiscountID, BookCount, Percentage, BookPriceID, Amount, State, Status, 
					CreatedAt, CreatedBy)
					Values (0, @PhaseID, @PhaseID, @BookID, @SupplierId, @DepartmentID, @PreviousDiscountID, 
					@BookCount, @Percentage, @BookpRiceID, @CatalogAmount, 0 , 1, GETDATE(), 'sysadmin')
			End
		END
		ELSE
		Begin
			SET @TotalBookCount = 0
			SET @TotalBookCount += @BookCount
			SET @PreviousDiscountID = 0
			SET @PreviousDiscountTo = 0
			SET @PreviousBookDiscountPercentage = 1

			Select @BookDiscountID = ISNULL(ID,0), @BookDiscountFrom = BookCountFrom, @BookDiscountTo = BookCountTo, @BookDiscountPercentage = ISNULL(DiscountPercentage, -1) 
					from Discount where BookID = @BookID and PhaseID = @myPhaseID  and @TotalBookCount >= BookCountFrom and @TotalBookCount <= BookCountTo

			SET @CatalogAmount = @BookCount * @BookUnitPrice * IIF(@BookDiscountPercentage > 0, @BookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage 

			Insert Into Catalog (CatalogType, PhaseID, OriginalPhaseID, BookID, SupplierID, DepartmentID, DiscountID, BookCount, Percentage, BookPriceID, Amount, State, Status, 
			CreatedAt, CreatedBy)
			Values (0, @PhaseID, @PhaseID, @BookID, @SupplierId, @DepartmentID,CASE WHEN @BookDiscountID > 0 THEN @BookDiscountID ELSE @YearDiscountID END, 
			@BookCount, @Percentage, @BookpRiceID, @CatalogAmount, 0, 1, GETDATE(), 'sysadmin')

			SET @PreviousDiscountID = CASE WHEN @BookDiscountID > 0 THEN @BookDiscountID ELSE @YearDiscountID END
			SET @PreviousBookId = @BookID
			SET @PreviousDiscountFrom = @BookDiscountFrom
			SET @PreviousDiscountTo = @BookDiscountTo
			SET @PreviousBookDiscountPercentage = @BookDiscountPercentage
		End

		FETCH NEXT FROM ReceiptsCursor   
		INTO @BookID, @PhaseID, @DepartmentID, @BookCount, @BookPriceID, @SupplierID, @Percentage, @BookFirstRegistrationYear, @BookUnitPrice
	END   
	CLOSE ReceiptsCursor;  
	DEALLOCATE ReceiptsCursor;  


	--Update Catalog 
	--SET Amount = bp.Price * tc.Bookcount * d.DiscountPercentage
	--from Catalog tc
	--inner join BookPrice bp on bp.ID = tc.BookPRiceId
	--inner join Discount d on d.ID = tc.DiscountId

	COMMIT TRANSACTION
	End Try
	Begin Catch
		print @@ERROR
        if @@Trancount > 0 ROLLBACK TRANSACTION
    END CATCH
END
GO


