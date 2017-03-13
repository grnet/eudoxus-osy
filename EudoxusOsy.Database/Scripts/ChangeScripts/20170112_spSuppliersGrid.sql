CREATE
PROCEDURE [report].[spSuppliersGrid] @phaseId INT AS

DECLARE @currentYear INT

SELECT @currentYear = Year FROM Phase WHERE ID = @phaseId

SELECT supdata.ID, supdata.Name, supdata.supplierType, supjoin.books_registered, ROUND(supjoin.totalPrice,2) as totalPrice, 
ROUND(statsFull.totalpayment,2) as totalpayment, ROUND(statsFull.totaltoyde,2) as totaltoyde, supjoin.books_registeredNOTPRICED  
FROM Supplier supdata
JOIN (
		SELECT books_registered, books_registeredNOTPRICED, totalPrice, priced.supplier_id 
		FROM
		(			
				SELECT SUM(books_registered) as books_registered, SUM(innerq1.books_registered*price*(percentage/100)) as totalPrice, supplier_id
				FROM
				(				
					SELECT SUM(times_registered) as books_registered, bs.SupplierID AS supplier_id, book_id, percentage
					FROM report.StatsPricedPerDepartment 
					JOIN BookSupplier bs ON bs.BookID = StatsPricedPerDepartment.book_id
					WHERE StatsPricedPerDepartment.phase_id = @phaseId AND bs.year = @currentYear AND bs.IsActive = 1
					GROUP BY book_id, bs.SupplierID, percentage 
					
				) as innerq1
			JOIN BookPrice ON BookPrice.BookID = innerq1.book_id
			WHERE BookPrice.year = @currentYear AND BookPrice.status = 1 
			GROUP BY supplier_id 			
		) priced
		LEFT JOIN (					
			SELECT SUM(books_registered) as books_registeredNOTPRICED, supplier_id
				FROM
				(				
					SELECT SUM(times_registered) as books_registered, bs.SupplierID AS supplier_id, book_id
					FROM report.StatsNotPricedPerDepartment 
					JOIN BookSupplier bs ON bs.BookID = StatsNotPricedPerDepartment.book_id
					WHERE StatsNotPricedPerDepartment.phase_id = @phaseId AND bs.year = @currentYear AND bs.IsActive = 1
					GROUP BY book_id, bs.SupplierID 
					
				) as innerq1
			JOIN BookPrice ON BookPrice.BookID = innerq1.book_id
			WHERE BookPrice.year = @currentYear AND BookPrice.status = 1
			GROUP BY supplier_id 
						
		) notpriced ON priced.supplier_id = notpriced.supplier_id
) supjoin ON supjoin.supplier_id = supdata.ID
JOIN report.SuppliersFullStatistics statsFull ON statsFull.supplier_id = supdata.Id 
WHERE supdata.ID = supjoin.supplier_id AND 
	supdata.ID = statsFull.supplier_id AND 	
	supdata.status = 1 AND
	statsFull.phase_id = @phaseId
ORDER BY supdata.ID ASC