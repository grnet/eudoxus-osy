USE [EudoxusOsy]
GO
/****** Object:  StoredProcedure [dbo].[CreateCatalogsForPhase]    Script Date: 22/02/2017 4:51:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
			@IsCoPublished int,
			@CoPublisherNum int,

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
			@PreviousBookDiscountPercentage decimal(5,2),
			@PreviousSupplierID int,			
			@PreviousTotalBooks int		

	SET @TotalBookCount = 0 
	SET	@PreviousBookID = 0
	Set @PreviousDiscountTo = 0
	Set @PreviousDiscountFrom = 0
	SET	@PreviousDepartmentID = 0
	SET @SubBookCountLow = 0
	SET @SubBookCountHigh = 0
	SET @CatalogAmount = 0	
	SET @PreviousSupplierID = 0
	SET @PreviousTotalBooks = 0

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
	from Receipt where PhaseID = @myPhaseID AND State = 1 AND Status = 0

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
		bp.Price,
		CASE WHEN bs.Percentage < 100 THEN 1 ELSE 0 END AS IsCoPublished,
		ROW_NUMBER() OVER (PARTITION BY tr.BookID, DepartmentID ORDER BY tr.BookID, DepartmentID, bs.SupplierID) AS  CoPublisherNum 
	from tempReceipt tr
	inner join Phase p on p.ID = tr.PhaseID
	left join BookPrice bp on bp.BookID = tr.BookID and bp.Status = 1 and bp.Year = p.Year
	inner join BookSupplier bs on bs.BookID = tr.BookID and bs.Year = p.Year and bs.IsActive = 1
	inner join Book b on b.ID = tr.BookID AND b.BookType <> 2 AND b.IsActive = 1			
	Group By tr.PhaseID, tr.BookID, DepartmentID, Bp.ID, Bs.SupplierID, Bs.Percentage, b.FirstRegistrationYear, bp.Price
	Order by tr.BookID, DepartmentID, bs.SupplierID
  
	OPEN ReceiptsCursor  
  	BEGIN TRY
	BEGIN TRANSACTION

	FETCH NEXT FROM ReceiptsCursor   
	INTO @BookID, @PhaseID, @DepartmentID, @BookCount, @BookPriceID, @SupplierID, @Percentage, @BookFirstRegistrationYear, @BookUnitPrice, @IsCoPublished, @CoPublisherNum 
  
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	-- For new condition of discount according to FirstRegistrationPhase
	-- Reduce price for 3% for every year after 5 years of FirstRegistration until 15% is reached
		if (ISNULL(@BookFirstRegistrationYear, 0) > 0) AND ((@SelectedPhaseYear - @BookFirstRegistrationYear) > 5)
		Begin
			Select @PerYearDiscountPercentage = 1 - (
			IIF(
			IIF(IIF(@SelectedPhaseYear - @BookFirstRegistrationYear - 8 > 0, @SelectedPhaseYear - @BookFirstRegistrationYear - 8, 0) > 5, 5, IIF(@SelectedPhaseYear - @BookFirstRegistrationYear - 8 > 0, @SelectedPhaseYear - @BookFirstRegistrationYear - 8, 0)) > 5,
			5,
			IIF(IIF(@SelectedPhaseYear - @BookFirstRegistrationYear - 8 > 0, @SelectedPhaseYear - @BookFirstRegistrationYear - 8, 0) > 5, 5, IIF(@SelectedPhaseYear - @BookFirstRegistrationYear - 8 > 0, @SelectedPhaseYear - @BookFirstRegistrationYear - 8, 0)))
			* 0.02)		
		end
		ELSE
		Begin
			Select @PerYearDiscountPercentage = 1
		End

		if @PreviousBookID = @BookID
		BEGIN
			if @IsCoPublished = 1 AND @CoPublisherNum > 1 AND @PreviousSupplierID <> @SupplierID	
			BEGIN
				SET @TotalBookCount = @PreviousTotalBooks 
				--print 'a ' + CONVERT(VARCHAR(50),@TotalBookCount) + ' sup ' + CONVERT(VARCHAR(50),@SupplierID)
			END
			ELSE IF @IsCoPublished = 1 AND @CoPublisherNum = 1 AND @PreviousSupplierID <> @SupplierID	
			BEGIN				
				SET @PreviousTotalBooks += @BookCount				
				SET @TotalBookCount = @PreviousTotalBooks
				--print 'b ' + CONVERT(VARCHAR(50),@TotalBookCount) + ' sup ' + CONVERT(VARCHAR(50),@SupplierID)
			END
			ELSE
			BEGIN
				SET @TotalBookCount += @BookCount
				--print 'c ' + CONVERT(VARCHAR(50),@TotalBookCount) + ' sup ' + CONVERT(VARCHAR(50),@SupplierID)
			END			
			
			--print 'Condition ' + ' book count' + CONVERT(VARCHAR(50),@BookCount) + ' total ' + CONVERT(VARCHAR(50),@TotalBookCount) + ' prev Disc ' + CONVERT(VARCHAR(50),@PreviousDiscountTo)
			
			if (@TotalBookCount > @PreviousDiscountTo) AND (@PreviousDiscountTo <> 0)
			Begin
				---------------------------------
				IF (@IsCoPublished = 1 AND @CoPublisherNum = 1) OR @IsCoPublished = 0 
				BEGIN				
					SET @PreviousDiscountID = @BookDiscountID 
					SET @PreviousDiscountFrom = @BookDiscountFrom
					SET @PreviousDiscountTo = @BookDiscountTo
					SET @PreviousBookDiscountPercentage = @BookDiscountPercentage
				END
				---------------------------------
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
				---------------------------------

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
							
			End
			Else
			Begin
				SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@BookCount,@BookUnitPrice,@Percentage,@BookDiscountPercentage,@YearDiscountPercentage,@PerYearDiscountPercentage)
				EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @PreviousDiscountID, @BookCount, @Percentage, @BookPriceID, @CatalogAmount
			End

			SET @PreviousSupplierID = @SupplierID
		END
		ELSE
		Begin
			SET @PreviousTotalBooks = 0			
			SET @TotalBookCount = @BookCount			
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
			SET @PreviousSupplierID = @SupplierID	
		End

		FETCH NEXT FROM ReceiptsCursor   
		INTO @BookID, @PhaseID, @DepartmentID, @BookCount, @BookPriceID, @SupplierID, @Percentage, @BookFirstRegistrationYear, @BookUnitPrice, @IsCoPublished, @CoPublisherNum 
	END   
	CLOSE ReceiptsCursor;  
	DEALLOCATE ReceiptsCursor;  

	UPDATE Receipt SET Status = 1	
	FROM Receipt		
	inner join Phase p on p.ID = Receipt.PhaseID
	inner join BookSupplier bs on bs.BookID = Receipt.BookID and bs.Year = p.Year and bs.IsActive = 1
	inner join Book b on b.ID = Receipt.BookID AND b.BookType <> 2 AND b.IsActive = 1
	where Receipt.State = 1 AND Receipt.Status = 0 AND Receipt.PhaseID = @myPhaseID

	UPDATE Phase SET TotalDebt = (SELECT SUM(Amount) FROM Catalog WHERE PhaseID = @PhaseID)	
	WHERE ID = @PhaseID
  
	print 'Complete!'

	COMMIT TRANSACTION
	End Try
	Begin Catch
		print @@ERROR
        if @@Trancount > 0 ROLLBACK TRANSACTION
    END CATCH
END
