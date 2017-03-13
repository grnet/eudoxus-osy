
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
			@YearDiscountPercentage decimal(5,2),
			@BookPriceID int,
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

	-- If there is no basic discount for the selected Phase then Create it
	-- The new rule for 2016 and beyond is there is no default discount on the prices
	IF @YearDiscountID = 0 
	Begin
		Insert Into Discount (PhaseID,BookCountFrom, BookCountTo, DiscountPercentage)
		Values (@myPhaseID, 0, 0, 1)
	End
	Select @YearDiscountID = IsNull(ID,0), @YearDiscountPercentage = ISNULL(DiscountPercentage,1) from Discount where PhaseID = @myPhaseID and BookID is null

	Select @SelectedPhaseYear = Year from Phase where ID = @myPhaseID

	IF EXISTS(SELECT name FROM sys.sysobjects WHERE Name = N'tempReceipt' AND xtype = N'U')
	BEGIN 
		DROP table tempReceipt
	END

	Select * into tempReceipt 
	from Receipt where PhaseID = @myPhaseID and Status = 1

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
	inner join Book b on b.ID = tr.BookID AND b.BookType <> 2 AND b.IsActive = 1
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

				IF @@rowcount = 0
				BEGIN
						Set @BookDiscountID = 0
						SET @BookDiscountFrom = 0
						SET @BookDiscountTo = 0
						SET @BookDiscountPercentage = -1
				END

				if @BookDiscountID = 0 
				Begin 
					Select @BookDiscountID = ISNULL(ID,0), @BookDiscountFrom = BookCountFrom, @BookDiscountTo = BookCountTo, @BookDiscountPercentage = ISNULL(DiscountPercentage, -1)
					from Discount where BookID IS null and PhaseID = @myPhaseID and @TotalBookCount >= BookCountFrom and @TotalBookCount <= BookCountTo
				End

				if @TotalBookCount - @BookCount < @BookDiscountFrom
				Begin
					SET @SubBookCountLow = @BookCount -  (@TotalBookCount - @BookDiscountFrom) - 1
					SET @SubBookCountHigh = @BookCount - @SubBookCountLow 

					if @SubBookCountLow > 0
					begin
						SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@SubBookCountLow,@BookUnitPrice,@Percentage,@PreviousBookDiscountPercentage,@YearDiscountPercentage,@PerYearDiscountPercentage )

						EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID,@BookID, @SupplierID, @DepartmentID, @PreviousDiscountID, @SubBookCountLow, @Percentage, @BookPriceID, @CatalogAmount
					end

					SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@SubBookCountHigh,@BookUnitPrice,@Percentage,@BookDiscountPercentage,@YearDiscountPercentage,@PerYearDiscountPercentage )

					EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @BookDiscountID, @SubBookCountHigh, @Percentage, @BookPriceID, @CatalogAmount
				End
				Else
				Begin
						SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@BookCount,@BookUnitPrice,@Percentage,@BookDiscountPercentage,@YearDiscountPercentage,@PerYearDiscountPercentage )

						EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @BookDiscountID, @BookCount, @Percentage, @BookPriceID, @CatalogAmount
				End

			SET @PreviousDiscountID = @BookDiscountID 
			SET @PreviousDiscountFrom = @BookDiscountFrom
			SET @PreviousDiscountTo = @BookDiscountTo
			SET @PreviousBookDiscountPercentage = @BookDiscountPercentage

			End
			Else
			Begin
					SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@BookCount,@BookUnitPrice,@Percentage,@BookDiscountPercentage,@YearDiscountPercentage,@PerYearDiscountPercentage)

					EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @PreviousDiscountID, @BookCount, @Percentage, @BookPriceID, @CatalogAmount
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
			
			IF @@rowcount = 0
				BEGIN
						Set @BookDiscountID = 0
						SET @BookDiscountFrom = 0
						SET @BookDiscountTo = 0
						SET @BookDiscountPercentage = -1
				END

			if @BookDiscountID = 0 
				Begin 
					Select @BookDiscountID = ISNULL(ID,0), @BookDiscountFrom = BookCountFrom, @BookDiscountTo = BookCountTo, @BookDiscountPercentage = ISNULL(DiscountPercentage, -1)
					from Discount where BookID IS null and PhaseID = @myPhaseID and @TotalBookCount >= BookCountFrom and @TotalBookCount <= BookCountTo
				End

			SET @CatalogAmount = @BookCount * Round((@Percentage/100) *  @BookUnitPrice * IIF(@BookDiscountPercentage > 0, @BookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage,2) 

			DECLARE @tmpDiscount INT
			SET @tmpDiscount = CASE WHEN @BookDiscountID > 0 THEN @BookDiscountID ELSE @YearDiscountID END

			SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@BookCount,@BookUnitPrice,@Percentage,@BookDiscountPercentage,@YearDiscountPercentage,@PerYearDiscountPercentage )

			EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @tmpDiscount, @BookCount, @Percentage, @BookPriceID, @CatalogAmount

			SET @PreviousDiscountID = @tmpDiscount
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

	COMMIT TRANSACTION
	End Try
	Begin Catch
		print @@ERROR
        if @@Trancount > 0 ROLLBACK TRANSACTION
    END CATCH
END
GO

--EXEC [dbo].[CreateCatalogsForPhase] 12