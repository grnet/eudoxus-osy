CREATE
PROCEDURE [report].[spBasicStats] 
	@phaseId INT

AS

DECLARE @notPricedBooks INT
DECLARE @currentYear INT
DECLARE @TotalBooks INT
DECLARE @pricedBooks INT
DECLARE @avgPricedBooks decimal(10,2)
DECLARE @TotalCost decimal(10,2)
DECLARE @ToYde decimal(10,2)


SELECT @currentYear = Year FROM Phase WHERE ID = @phaseId

--Σύνολο Βιβλίων: 51468
--Σύνολο Κοστολογημένων Βιβλίων: 28523
--Μέση τιμή κοστολογημένων βιβλίων: €18,39
--Συνολικό κόστος βιβλίων που έχουν παραληφθεί: €0,00
--Συνολικό ποσό προς ΥΔΕ: €0,00

DECLARE @currentPhaseID int
	SELECT @currentPhaseID = ID from Phase p where p.IsActive = 1 and EndDate is null

SET @totalBooks = @pricedBooks + @notPricedBooks

SELECT @totalBooks = COUNT(*) 
FROM Book 
WHERE IsActive = 1 

SELECT @pricedBooks = COUNT(*) 
FROM BookPrice 
WHERE status = 1 AND price != 0 AND year = @currentYear

SELECT @avgPricedBooks = AVG(price) 
FROM BookPrice 
WHERE STATUS = 1 AND price <> 0 AND year = @currentYear

IF @phaseId = @currentPhaseID
BEGIN
SELECT @totalCost = SUM(debt)
FROM   report.ViewStatisticsPerDepartment 
WHERE PhaseId = @phaseId  
END

IF @phaseId <> @currentPhaseID
BEGIN
SELECT @totalCost = SUM(debt)
FROM   report.ViewStatisticsPerDepartment_PP 
WHERE PhaseId = @phaseId  
END

IF @phaseId = @currentPhaseID
BEGIN
SELECT @toyde = SUM(totaltoyde)
FROM report.SuppliersFullStatistics
WHERE phase_id = @phaseId
END

IF @phaseId <> @currentPhaseID
BEGIN
SELECT @toyde = SUM(totaltoyde)
FROM report.SuppliersFullStatistics_PP
WHERE phase_id = @phaseId
END


SELECT @totalBooks AS totalbooks ,@pricedBooks as pricedBooks ,@avgPricedBooks as avgPricedBooks ,@totalCost as TotalCost, @toyde as TotalToYDE
GO


