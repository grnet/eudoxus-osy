ALTER PROCEDURE [dbo].[sp_GetSuppliersStatsForExport] @phaseID int
AS
BEGIN
DECLARE @currentPhaseID int
SELECT @currentPhaseID = ID from Phase p where p.IsActive = 1 and EndDate is null

IF(@phaseID = @currentPhaseID)
	exec report.spSuppliersGrid @phaseID
ELSE
	SELECT supplier_kpsid as ID, publicFinancialOffice as Name, supplierType, totalprice, totalpayment, totaltoyde, booksPriced,
		booksNotPriced, paymentPfo, taxRoll_number, phase_id as phaseId
	 from report.SuppliersFullStatistics_PP where (@phaseID = 0 OR phase_id = @phaseID)
END