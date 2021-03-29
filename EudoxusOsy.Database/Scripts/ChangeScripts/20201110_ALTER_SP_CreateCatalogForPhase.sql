ALTER PROCEDURE [dbo].[CreateCatalogsForPhase]
@myPhaseID int
AS
Begin
	DECLARE @BookID int,
	@DepartmentID int,@BookCount int,
	@BookDiscountID int,@BookDiscountFrom int,
	@BookDiscountTo int,@YearDiscountID int,
	@YearDiscountPercentage decimal(5,2),@BookPriceID int,
	@PhaseID int,@SupplierID int,@Percentage decimal(5,2),
	@BookFirstRegistrationYear int,@BookUnitPrice decimal(5,2),
	@BookDiscountPercentage decimal(5,2),@IsCoPublished int,
	@CoPublisherNum int,@TotalBookCount int,@CatalogAmount decimal(8,2), 			
	@SelectedPhaseYear int,@PerYearDiscountPercentage decimal(5,2)
	
	DECLARE @PreviousBookCount INT
	DECLARE @Low INT, @DiscountIDLow INT, @LowDiscountPercentage decimal(5,2)
	DECLARE @Mid INT, @DiscountIDMid INT, @MidDiscountPercentage decimal(5,2)
	DECLARE @High INT, @DiscountIDHigh INT, @HighDiscountPercentage decimal(5,2)
	
	SET @LowDiscountPercentage = 1
	SET @MidDiscountPercentage = 0.95
	SET @HighDiscountPercentage = 0.9
	DECLARE @limit_1 INT
	DECLARE @limit_2 INT
	DECLARE @diff_years INT
	SET @limit_1 = 800
	SET @limit_2 = 1500
	SELECT @DiscountIDLow = ID FROM Discount WHERE PhaseID = @myPhaseID AND BookCountTo = @limit_1
	SELECT @DiscountIDMid = ID FROM Discount WHERE PhaseID = @myPhaseID AND BookCountTo = @limit_2
	SELECT @DiscountIDHigh = ID FROM Discount WHERE PhaseID = @myPhaseID AND BookCountFrom = @limit_2 + 1	
						
	Select @SelectedPhaseYear = Year from Phase where ID = @myPhaseID
	-----------------------------------------------			
	IF EXISTS(SELECT name FROM sys.sysobjects WHERE Name = N'tempReceipt' AND xtype = N'U')
	BEGIN
		DROP table dbo.tempReceipt
	END
	IF EXISTS(SELECT name FROM sys.sysobjects WHERE Name = N'tempCatalog' AND xtype = N'U')
	BEGIN
		DROP table dbo.tempCatalog
	END
	Select * into dbo.tempReceipt
	from Receipt where PhaseID = @myPhaseID AND State = 1 AND Status = 0
	
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
		into dbo.tempCatalog
	from dbo.tempReceipt tr
	inner join Phase p on p.ID = tr.PhaseID
	left join BookPrice bp on bp.BookID = tr.BookID and bp.Status = 1 and bp.Year = p.Year
	inner join BookSupplier bs on bs.BookID = tr.BookID and bs.Year = p.Year and bs.IsActive = 1
	inner join Book b on b.ID = tr.BookID AND b.BookType <> 2						
	Group By tr.PhaseID, tr.BookID, DepartmentID, Bp.ID, Bs.SupplierID, Bs.Percentage, b.FirstRegistrationYear, bp.Price
	Order by tr.BookID, DepartmentID, bs.SupplierID
	DECLARE ReceiptsCursor CURSOR FOR
	SELECT *, sum(BookCount) OVER (PARTITION BY BookID, SupplierID, CoPublisherNum ORDER BY  BookID, DepartmentID, SupplierID, CoPublisherNum) as TotalBookCount 	
	FROM dbo.tempCatalog
	Order by BookID, DepartmentID, SupplierID
	OPEN ReceiptsCursor
  	BEGIN TRY
	BEGIN TRANSACTION
	FETCH NEXT FROM ReceiptsCursor
	INTO @BookID, @PhaseID, @DepartmentID, @BookCount, @BookPriceID, @SupplierID, @Percentage, @BookFirstRegistrationYear, @BookUnitPrice, @IsCoPublished, @CoPublisherNum, @TotalBookCount
	WHILE @@FETCH_STATUS = 0
	BEGIN  	
		
		-- For new condition of discount according to FirstRegistrationPhase
		-- Reduce price for 2% for every year after 8 years of FirstRegistration until 10% is reached
		-- THIS PROBABLY NEEDS REFACTORING!!!
		SET @diff_years = @SelectedPhaseYear - @BookFirstRegistrationYear;
		if (ISNULL(@BookFirstRegistrationYear, 0) > 0) AND (@diff_years > 7)
		Begin			
			Select @PerYearDiscountPercentage = IIF((@diff_years-7)* 0.02 > 0.10, 0.10, (@diff_years-7)* 0.02)		
		end
		ELSE
		Begin
			Select @PerYearDiscountPercentage = 0
		End
		SET @PreviousBookCount = @TotalBookCount - @BookCount
		--Σκοπός του αλγόριθμου είναι να διανείμει σε κάθε επανάληωη
		--το πλήθος των βιβλίων σε τρία διαστήματα.
		--Τα διαστήματα για την περίοδο 13 είναι 3
		-- 0 - 800
		-- 801 - 1501
		-- 1501 -
		--!!!Σε περίπτωση που προστεθούν και άλλες γενικές εκπτώσεις ή εκπτώσεις ανά βιβλίο χρειάζεται αλλαγή στον αλγόριθμο!!!
		--Low
		--Το Low διάστημα αφορά πλήθος βιβλίων από 0 έως και @limit_1(800)
		--------------------------------------
		IF @TotalBookCount > @limit_1
		BEGIN
			--A--
			IF @limit_1 - @PreviousBookCount > 0
			BEGIN
				SET @Low = @BookCount - (@TotalBookCount - @limit_1)				
			END
			ELSE
			BEGIN
				SET @Low = 0
			END
			--A--
		END
		ELSE
		BEGIN
			SET @Low = @BookCount
		END
		--------------------------------------
		--Mid
		--Το Mid διάστημα αφορά πλήθος βιβλίων από @limit_1(801) έως και @limit_2(1500)
		--------------------------------------
		IF @TotalBookCount > @limit_1 AND @TotalBookCount <= @limit_2
		BEGIN
			--B--
			IF @PreviousBookCount > @limit_1
			BEGIN
				SET @Mid = @BookCount
			END
			ELSE
			BEGIN
				SET @Mid = @TotalBookCount - @limit_1				
			END
			--B--
		END
		ELSE IF @TotalBookCount > @limit_2
		BEGIN
			--A--
			IF @limit_2 - @PreviousBookCount > 0
			BEGIN
				SET @Mid = @BookCount - @Low - (@TotalBookCount - @limit_2)
			END
			ELSE
			BEGIN
				SET @Mid = 0
			END
			--A--
		END
		ELSE
		BEGIN
			SET @Mid = 0
		END
		--------------------------------------
		--High
		--Το High διάστημα αφορά πλήθος βιβλίων από @limit_2(1501) και μετα.
		--------------------------------------
		IF @TotalBookCount > @limit_2
		BEGIN
			--B--
			IF @PreviousBookCount > @limit_2
			BEGIN
				SET @High = @BookCount
			END
			ELSE
			BEGIN
				SET @High = @TotalBookCount - @limit_2
			END
			--B--
		END
		ELSE
		BEGIN
			SET @High = 0
		END
		--------------------------------------		
		--print 'Low : ' + CONVERT(VARCHAR(50),@Low) +
		--	' Mid : ' + CONVERT(VARCHAR(50),@Mid) +
		--	' High : ' + CONVERT(VARCHAR(50),@High) +
		--	' BookCount : ' + CONVERT(VARCHAR(50),@BookCount) +
		--	' BookUnitPrice : ' + CONVERT(VARCHAR(50),@BookUnitPrice) +
		--	' TotalBookCount : ' + CONVERT(VARCHAR(50),@TotalBookCount) 			
			
		IF @Low > 0
		BEGIN
			SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@Low,@BookUnitPrice,@Percentage,@LowDiscountPercentage,@PerYearDiscountPercentage )
			EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @DiscountIDLow, @Low, @Percentage, @BookPriceID, @CatalogAmount	
			INSERT INTO TEMP_CATALOG_TEST(BookId, Data) VALUES
				(@BookID, CONVERT(VARCHAR(100), @SupplierID) + ' '+ CONVERT(VARCHAR(100),  @DepartmentID) + ' Low '+  CONVERT(VARCHAR(100), @BookUnitPrice)
				+ ' '+ CONVERT(VARCHAR(100), @Percentage)+ ' '+ CONVERT(VARCHAR(100), @LowDiscountPercentage)+ ' '+ CONVERT(VARCHAR(100), @PerYearDiscountPercentage) )
		END
		IF @Mid > 0
		BEGIN
			SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@Mid,@BookUnitPrice,@Percentage,@MidDiscountPercentage,@PerYearDiscountPercentage )
			EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @DiscountIDMid, @Mid, @Percentage, @BookPriceID, @CatalogAmount	
			INSERT INTO TEMP_CATALOG_TEST(BookId, Data) VALUES
				(@BookID, CONVERT(VARCHAR(100), @SupplierID) + ' '+ CONVERT(VARCHAR(100),  @DepartmentID) + ' Mid '+  CONVERT(VARCHAR(100), @BookUnitPrice)
				+ ' '+ CONVERT(VARCHAR(100), @Percentage)+ ' '+ CONVERT(VARCHAR(100), @MidDiscountPercentage)+ ' '+ CONVERT(VARCHAR(100), @PerYearDiscountPercentage) )
		END
		IF @High > 0
		BEGIN
			SELECT @CatalogAmount = dbo.fnCalculateCatalogAmount(@High,@BookUnitPrice,@Percentage,@HighDiscountPercentage,@PerYearDiscountPercentage )
			EXEC [dbo].[spInsertIntoCatalogs] @PhaseID, @PhaseID, @BookID, @SupplierID, @DepartmentID, @DiscountIDHigh, @High, @Percentage, @BookPriceID, @CatalogAmount
				
			INSERT INTO TEMP_CATALOG_TEST(BookId, Data) VALUES
				(@BookID, CONVERT(VARCHAR(100), @SupplierID) + ' '+ CONVERT(VARCHAR(100),  @DepartmentID) + ' High '+  CONVERT(VARCHAR(100), @BookUnitPrice)
				+ ' '+ CONVERT(VARCHAR(100), @Percentage)+ ' '+ CONVERT(VARCHAR(100), @HighDiscountPercentage)+ ' '+ CONVERT(VARCHAR(100), @PerYearDiscountPercentage) )
		END
								
		FETCH NEXT FROM ReceiptsCursor
		INTO @BookID, @PhaseID, @DepartmentID, @BookCount, @BookPriceID, @SupplierID, @Percentage, @BookFirstRegistrationYear, @BookUnitPrice, @IsCoPublished, @CoPublisherNum, @TotalBookCount
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
		print error_message()
		print @@ERROR
        if @@Trancount > 0 ROLLBACK TRANSACTION
    END CATCH
END
GO