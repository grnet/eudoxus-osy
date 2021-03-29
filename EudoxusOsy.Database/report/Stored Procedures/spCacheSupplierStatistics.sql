

--EXEC [report].[spCacheSupplierStatistics] 10
CREATE 
PROCEDURE [report].[spCacheSupplierStatistics] @phaseId INT AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
   DECLARE @currentYear INT
   DECLARE @rows INT

   SELECT @currentYear = Year
   FROM Phase
   WHERE ID = @phaseId

   SET @rows = @@ROWCOUNT

   IF @rows = 0 OR @rows > 1    
		SET @currentYear = 2010 --????????????????????????
   
   --------------------------------------------------------------------------------
	--Find Books with price
   DELETE FROM report.SuppliersFullStatistics WHERE phase_id = @phaseId

   DECLARE @priced NVARCHAR(4000)
   DECLARE @notPriced NVARCHAR(4000)
   DECLARE @finalQuery NVARCHAR(4000)

	SET @priced =' ( 
					SELECT SupplierID, SUM(booksPriced) AS booksPriced, SUM(totalPrice) AS totalPrice
					FROM (
						SELECT COUNT (DISTINCT r.BookID) AS booksPriced, SUM(r.BookCount)*ROUND(MAX(price*(percentage/100)),2) as totalPrice, s.ID AS SupplierID, r.BookID
						FROM Receipt r
						JOIN Phase p ON p.ID = r.PhaseID
						JOIN BookSupplier bs ON bs.BookID = r.BookID AND bs.Year = p.Year AND bs.IsActive = 1
						JOIN Supplier s ON s.ID = bs.SupplierID 
						JOIN BookPrice bp ON bp.BookID = r.BookID AND bp.Year = p.Year AND bp.Status = 1
						WHERE r.PhaseID = ' + CONVERT(NVARCHAR(50),@phaseId) + ' AND p.IsActive = 1 AND r.State = 1 AND r.Status <> 2 
						GROUP BY s.ID, r.BookID
					) a
					GROUP BY SupplierID		  					
				) as priced  '

	SET @notPriced = ' ( 	SELECT SupplierID, SUM(booksNotPriced) AS booksNotPriced
							FROM(
								SELECT COUNT (DISTINCT r.BookID) AS booksNotPriced, s.ID AS SupplierID
								FROM Receipt r
								JOIN Phase p ON p.ID = r.PhaseID
								JOIN BookSupplier bs ON bs.BookID = r.BookID AND bs.Year = p.Year AND bs.IsActive = 1
								JOIN Supplier s ON s.ID = bs.SupplierID 
								LEFT JOIN BookPrice bp ON bp.BookID = r.BookID AND bp.Year = p.Year AND bp.Status = 1
								WHERE r.PhaseID = ' + CONVERT(NVARCHAR(50),@phaseId) + ' AND p.IsActive = 1 AND r.State = 1 AND r.Status <> 2 
								AND  bp.bookId IS NULL
								GROUP BY s.ID, r.BookID
							) b
							GROUP BY SupplierID							  
						  ) as notpriced '

	SET @finalQuery = 'INSERT INTO #suppliersFullStatistics(
			   supplier_id,supplier_kpsid,publicFinancialOffice,paymentPfo,contact_name, taxRoll_number,official_name, 
			   supplierType,totalprice,totalpayment,totalofferprice,totaltoyde,phase_id, booksPriced, booksNotPriced  )		 
		SELECT supdata.ID, supdata.SupplierKpsID, supdata.Name, supdata.PaymentPfo, rep.ContactName , supdata.AFM , supdata.TradeName,
			   supdata.SupplierType, supjoin.totalPrice, 0, 0, 0 , ' + CONVERT(NVARCHAR(50),@phaseId) +',booksPriced, booksNotPriced 
		FROM Supplier AS supdata
		JOIN Reporter rep ON rep.ID = supdata.ReporterID
		JOIN (
				SELECT priced.totalPrice , priced.SupplierID, booksPriced, booksNotPriced
				FROM ' + @priced + '
				LEFT JOIN ' + @notPriced + ' ON notpriced.SupplierID = priced.SupplierID
		) as supjoin ON supjoin.SupplierID = supdata.ID				  
		WHERE supdata.Status = 1 
		ORDER BY supdata.SupplierKpsID ASC'

		CREATE TABLE #suppliersFullStatistics (
		supplier_id INT,
		supplier_kpsid INT,
		publicFinancialOffice NVARCHAR(200),
		paymentPfo NVARCHAR(50),
		contact_name NVARCHAR(200),
		taxRoll_number NVARCHAR(20),
		official_name NVARCHAR(200),
		supplierType INT,
		totalprice  decimal(10,2),
		totalpayment  decimal(10,2),
		totalofferprice  decimal(10,2),
		totaltoyde  decimal(10,2),
		phase_id INT,
		booksPriced INT,
		booksNotPriced INT
	)	
	
	EXECUTE sp_executesql @finalQuery
	
	--print 'Final ' + CONVERT(VARCHAR(20), GETDATE()) + ' ' +  CONVERT(VARCHAR(200), @@ROWCOUNT)

	DECLARE @count INT 
	SET @count = 0	
	--SELECT @count = COUNT(*) FROM #suppliersFullStatistics
	--print 'Final ' +  CONVERT(VARCHAR(200), @count)
	--SET @count = 0

	DECLARE @Csupplier_id INT
	DECLARE @Csupplier_kpsid INT
	DECLARE @CpublicFinancialOffice NVARCHAR(200)
	DECLARE @CpaymentPfo NVARCHAR(50)
	DECLARE @Ccontact_name NVARCHAR(200)
	DECLARE @CtaxRoll_number NVARCHAR(20)
	DECLARE @Cofficial_name NVARCHAR(200)
	DECLARE @CsupplierType INT
	DECLARE @Ctotalprice  decimal(10,2)
	DECLARE @Ctotalpayment  decimal(10,2)
	DECLARE @Ctotalofferprice  decimal(10,2)
	DECLARE @Ctotaltoyde  decimal(10,2)
	DECLARE @Cphase_id INT
	DECLARE @CbooksPriced INT
	DECLARE @CbooksNotPriced INT
	
	DECLARE vend_cursor CURSOR  
	FOR 
	SELECT * FROM #suppliersFullStatistics
	OPEN vend_cursor  
	FETCH NEXT FROM vend_cursor
	INTO @Csupplier_id, @Csupplier_kpsid, @CpublicFinancialOffice, @CpaymentPfo, @Ccontact_name,
			@CtaxRoll_number, @Cofficial_name, @CsupplierType, @Ctotalprice, @Ctotalpayment, @Ctotalofferprice, @Ctotaltoyde, @Cphase_id, @CbooksPriced, @CbooksNotPriced 
	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE @money  decimal(10,2) 
        DECLARE @supplier_totaldebt  decimal(10,2)		
		DECLARE @totalPayment  decimal(10,2)
		DECLARE @toyde  decimal(10,2)		                        			

        SELECT @supplier_totaldebt = TotalDebt 
		FROM SupplierPhase 
		WHERE SupplierID = @Csupplier_id AND PhaseID = @Cphase_id AND IsActive = 1
                   		
        SELECT @money = SUM(c.SUM)  
		FROM CatalogGroup 
		JOIN (
			SELECT Catalog.GroupID, SUM(Catalog.BookCount * BookPrice.Price) AS SUM
            FROM Catalog
			JOIN BookPrice ON Catalog.BookID = BookPrice.BookID 
            WHERE BookPrice.Status = 1 
				AND Catalog.Status = 1 AND BookPrice.Year = @currentYear
			GROUP BY Catalog.GroupID
		) c ON c.GroupID = CatalogGroup.ID
		WHERE SupplierID = @Csupplier_id AND PhaseID = @Cphase_id AND state <> 0 AND IsActive = 1
                                                   
		IF @supplier_totaldebt > @money 
		BEGIN
			SET @totalPayment = @supplier_totaldebt
		END
		ELSE
		BEGIN
			SET @totalPayment = @money
		END			

        SELECT @toyde = SUM(c.Amount) 
		FROM Catalog c 
		JOIN CatalogGroup g ON c.GroupID = g.ID 
		WHERE c.SupplierID = @Csupplier_id AND g.state = 3 AND c.PhaseID = @Cphase_id AND c.Status = 1 AND g.IsActive = 1                   

		INSERT INTO report.SuppliersFullStatistics (supplier_id,supplier_kpsid,publicFinancialOffice ,paymentPfo ,contact_name ,taxRoll_number ,
											official_name ,supplierType ,totalprice ,
											totalpayment ,totalofferprice ,totaltoyde ,phase_id, booksPriced, booksNotPriced )
		VALUES(@Csupplier_id,@Csupplier_kpsid,@CpublicFinancialOffice,@CpaymentPfo,@Ccontact_name,@CtaxRoll_number,
											@Cofficial_name, @CsupplierType, @Ctotalprice, 
											@totalPayment, ISNULL(@Ctotalprice,0) - ISNULL(@totalPayment,0), @toyde, @phaseId, @CbooksPriced, @CbooksNotPriced )
			
		SET @count = @count + 1

		FETCH NEXT FROM vend_cursor
		INTO @Csupplier_id, @Csupplier_kpsid, @CpublicFinancialOffice, @CpaymentPfo, @Ccontact_name,
			@CtaxRoll_number, @Cofficial_name, @CsupplierType, @Ctotalprice, @Ctotalpayment, @Ctotalofferprice, @Ctotaltoyde, @Cphase_id, @CbooksPriced, @CbooksNotPriced 
	END
	CLOSE vend_cursor -- close the cursor
	DEALLOCATE vend_cursor -- Deallocate the cursor

	DROP TABLE #suppliersFullStatistics
END