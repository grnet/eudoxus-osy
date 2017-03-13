CREATE PROCEDURE sp_GetSuppliersStatsForExport @phaseID int
AS
BEGIN
DECLARE @currentPhaseID int
SELECT @currentPhaseID = ID from Phase p where p.IsActive = 1 and EndDate is null

IF(@phaseID = @currentPhaseID)
	exec report.spSuppliersGrid @phaseID
ELSE
	SELECT supplier_kpsid as ID, official_name as Name, supplierType, totalprice, totalpayment, totaltoyde, pricedBooksCount as books_registered,
		notPricedBooksCount as books_registeredNOTPRICED, paymentPfo, taxRoll_number
	 from report.SuppliersFullStatistics_PP where phase_id = @phaseID
END