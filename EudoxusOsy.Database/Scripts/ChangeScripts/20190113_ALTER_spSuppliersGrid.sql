ALTER
PROCEDURE [report].[spSuppliersGrid] @phaseId INT AS

DECLARE @currentYear INT

SELECT @currentYear = Year FROM Phase WHERE ID = @phaseId

SELECT supdata.ID, supdata.Name, supdata.supplierType, booksPriced, totalPrice as totalPrice, 
ROUND(statsFull.totalpayment,2) as totalpayment, ROUND(statsFull.totaltoyde,2) as totaltoyde, booksNotPriced,
statsFull.phase_id as phaseId
FROM Supplier supdata
JOIN report.SuppliersFullStatistics statsFull ON statsFull.supplier_id = supdata.Id 
WHERE supdata.ID = statsFull.supplier_id AND 	
	supdata.status = 1 AND
	statsFull.phase_id = @phaseId
ORDER BY supdata.ID ASC
