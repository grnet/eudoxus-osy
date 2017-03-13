CREATE
PROCEDURE [report].[spSuppliersFullStatistics_PP] AS

TRUNCATE TABLE report.SuppliersFullStatistics_PP    

DECLARE @currentPhaseID int 
SELECT @currentPhaseID = ID from Phase where IsActive = 1 and EndDate is null
    
INSERT INTO report.SuppliersFullStatistics_PP (
supplier_id,supplier_kpsid,publicFinancialOffice,paymentPfo,contact_name,taxRoll_number,official_name,supplierType,totalprice,
totalpayment,totalofferprice,totaltoyde,status,phase_id,pricedBooksCount,notPricedBooksCount)
SELECT cp.SupplierID,s.SupplierKpsID,s.Name,s.PaymentPfo,s.Email,s.AFM,s.TradeName, s.SupplierType, cp.totalMoney,
cp.givenMoney,0,gp.sentMoney,1, cp.PhaseID, cp.pricedBooks, cp.notPricedBooks
    FROM (
			SELECT catalog.SupplierID,
					catalog.PhaseID,
					ROUND(SUM(catalog.Amount), 2) AS totalMoney,
					ROUND(MAX(ISNULL(supplierPhase.TotalDebt, 0)), 2) AS givenMoney,						   
					COUNT(DISTINCT(catalog.BookPriceID)) AS pricedBooks,
					COUNT(DISTINCT(catalog.BookID)) - COUNT(DISTINCT(catalog.BookPriceID)) AS notPricedBooks
				FROM Catalog
				JOIN Phase PP on PP.ID = catalog.PhaseID and PP.ID <> @currentPhaseID
				JOIN Supplier ON Supplier.ID = Catalog.SupplierID
				LEFT JOIN SupplierPhase ON SupplierPhase.SupplierID = Catalog.SupplierID AND
												supplierPhase.PhaseID = catalog.PhaseID AND
												supplierPhase.IsActive = 1
				WHERE catalog.Status = 1 AND catalog.State IN (0,1)						
				GROUP BY catalog.SupplierID, catalog.PhaseID
				--ORDER BY catalog.SupplierID, catalog.PhaseID
    ) AS cp	
	JOIN Supplier s ON s.ID = cp.SupplierID 
    LEFT JOIN (
			SELECT catalog.SupplierID,
					catalog.PhaseID,
					ROUND(SUM(Amount), 2) AS sentMoney
				FROM Catalog
				JOIN Phase PP on PP.ID = catalog.PhaseID and PP.ID <> @currentPhaseID
				LEFT JOIN CatalogGroup ON Catalog.GroupID = CatalogGroup.ID AND CatalogGroup.SupplierID = Catalog.SupplierID
				WHERE catalog.status = 1
				AND catalog.state IN (0,1)                    
				AND CatalogGroup.IsActive = 1
				AND CatalogGroup.state = 3                    
				GROUP BY catalog.SupplierID, catalog.PhaseID
				--ORDER BY catalog.SupplierID, catalog.PhaseID
    ) as gp ON cp.SupplierID = gp.SupplierID AND cp.PhaseID = gp.PhaseID      
    

	--SELECT * FROM report.SuppliersFullStatistics_PP

GO


