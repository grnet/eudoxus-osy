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
   
   DELETE FROM report.SuppliersFullStatistics WHERE phase_id = @phaseId

   DECLARE @query1 NVARCHAR(4000)
   DECLARE @query2 NVARCHAR(4000)
   DECLARE @priced NVARCHAR(4000)
   DECLARE @notPriced NVARCHAR(4000)
   DECLARE @finalQuery NVARCHAR(4000)

   SET @query1 = ' SELECT SUM(times_registered) as books_registered, SupplierID, book_id, percentage
                   FROM report.StatsPricedPerDepartment 
				   JOIN BookSupplier bs ON bs.BookID = book_id
                   WHERE phase_id = '+ CONVERT(NVARCHAR(50), @phaseId) + ' AND bs.year = ' + CONVERT(NVARCHAR(50),@currentYear) + ' AND bs.IsActive = 1
                   GROUP BY book_id, SupplierID, percentage '

   SET @query2 = ' SELECT SUM(times_registered) as books_registered, SupplierID, book_id
                   FROM report.StatsNotPricedPerDepartment 
				   JOIN BookSupplier bs ON bs.BookID = book_id
			       WHERE phase_id = ' + CONVERT(NVARCHAR(50),@phaseId) + ' AND bs.year = ' + CONVERT(NVARCHAR(50),@currentYear) + ' AND bs.IsActive = 1
		           GROUP BY book_id, SupplierID '

	SET @priced =' ( SELECT SUM(books_registered) as books_registered, SUM(innerq1.books_registered*price*(percentage/100)) as totalPrice, SupplierID
						FROM
						(
							' + @query1 + ' 
						) as innerq1
						LEFT JOIN BookPrice bk ON bk.BookID = innerq1.book_id
						WHERE bk.year = ' + CONVERT(NVARCHAR(50),@currentYear) + ' AND bk.Status = 1
						GROUP BY SupplierID 			  					
				) as priced  '

	SET @notPriced = ' ( 	SELECT SUM(books_registered) as books_registeredNOTPRICED, SupplierID
								FROM
								(
									' + @query2 + ' 
								) as innerq1
								LEFT JOIN BookPrice bk ON bk.BookID = innerq1.book_id
								WHERE bk.year = ' + CONVERT(NVARCHAR(50),@currentYear) + ' AND bk.Status = 1
								GROUP BY SupplierID 							  
						  ) as notpriced '

	SET @finalQuery = 'INSERT INTO #suppliersFullStatistics(
			   supplier_id,supplier_kpsid,publicFinancialOffice,paymentPfo,contact_name, taxRoll_number,official_name, 
			   supplierType,totalprice,totalpayment,totalofferprice,totaltoyde,phase_id )		 
		SELECT supdata.ID, supdata.SupplierKpsID, supdata.Name, supdata.PaymentPfo, supdata.Name , supdata.AFM , supdata.TradeName,
			   supdata.SupplierType, supjoin.totalPrice, 0, 0, 0 , ' + CONVERT(NVARCHAR(50),@phaseId) +'
		FROM Supplier AS supdata
		JOIN (
				SELECT priced.totalPrice , priced.SupplierID
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
		totalprice float,
		totalpayment float,
		totalofferprice float,
		totaltoyde float,
		phase_id INT
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
	DECLARE @Ctotalprice float
	DECLARE @Ctotalpayment float
	DECLARE @Ctotalofferprice float
	DECLARE @Ctotaltoyde float
	DECLARE @Cphase_id INT
	
	DECLARE vend_cursor CURSOR  
	FOR 
	SELECT * FROM #suppliersFullStatistics
	OPEN vend_cursor  
	FETCH NEXT FROM vend_cursor
	INTO @Csupplier_id, @Csupplier_kpsid, @CpublicFinancialOffice, @CpaymentPfo, @Ccontact_name,
			@CtaxRoll_number, @Cofficial_name, @CsupplierType, @Ctotalprice, @Ctotalpayment, @Ctotalofferprice, @Ctotaltoyde, @Cphase_id 
	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE @money FLOAT 
        DECLARE @supplier_totaldebt FLOAT
		DECLARE @supplierId INT
		DECLARE @totalPayment FLOAT
		DECLARE @toyde FLOAT

        SELECT @supplierId = ID
		FROM Supplier 
		WHERE SupplierKpsID = @Csupplier_kpsid 
			AND Status = 1
        
		SET @rows = @@ROWCOUNT

        IF @rows = 1
        BEGIN			                        			

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

			--print 'money ' + CONVERT(VARCHAR(20), GETDATE())

            SELECT @toyde = SUM(c.Amount) 
			FROM Catalog c 
			JOIN CatalogGroup g ON c.GroupID = g.ID 
			WHERE c.SupplierID = @Csupplier_id AND g.state = 3 AND c.PhaseID = @Cphase_id AND c.Status = 1 AND g.IsActive = 1                   

			INSERT INTO report.SuppliersFullStatistics (supplier_id,supplier_kpsid,publicFinancialOffice ,paymentPfo ,contact_name ,taxRoll_number ,
												official_name ,supplierType ,totalprice ,
												totalpayment ,totalofferprice ,totaltoyde ,phase_id)
			VALUES(@Csupplier_id,@Csupplier_kpsid,@CpublicFinancialOffice,@CpaymentPfo,@Ccontact_name,@CtaxRoll_number,
												@Cofficial_name, @CsupplierType, @Ctotalprice, 
												@totalPayment, @Ctotalprice - @totalPayment, @toyde, @phaseId)
			
			SET @count = @count + 1
			--print '---------------- ' + CONVERT(VARCHAR(20),@count) + ' ------------------'
		END	

		FETCH NEXT FROM vend_cursor
		INTO @Csupplier_id, @Csupplier_kpsid, @CpublicFinancialOffice, @CpaymentPfo, @Ccontact_name,
			@CtaxRoll_number, @Cofficial_name, @CsupplierType, @Ctotalprice, @Ctotalpayment, @Ctotalofferprice, @Ctotaltoyde, @Cphase_id 
	END
	CLOSE vend_cursor -- close the cursor
	DEALLOCATE vend_cursor -- Deallocate the cursor

	DROP TABLE #suppliersFullStatistics
END