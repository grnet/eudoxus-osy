ALTER
--CREATE
PROCEDURE [report].[spBasicStats] 
	@phaseId INT,
	@totalBooks INT OUTPUT,
	@pricedBooks INT OUTPUT,
	@avgPricedBooks FLOAT OUTPUT,
	@totalCost FLOAT OUTPUT,
	@toyde FLOAT OUTPUT

AS

DECLARE @notPricedBooks INT
DECLARE @currentYear INT

SELECT @currentYear = Year FROM Phase WHERE ID = @phaseId

--Σύνολο Βιβλίων: 51468
--Σύνολο Κοστολογημένων Βιβλίων: 28523
--Μέση τιμή κοστολογημένων βιβλίων: €18,39
--Συνολικό κόστος βιβλίων που έχουν παραληφθεί: €0,00
--Συνολικό ποσό προς ΥΔΕ: €0,00

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

SELECT @totalCost = SUM(debt)
FROM ViewStatisticsPerDepartment
WHERE phase_id = @phaseId

SELECT @toyde = SUM(totaltoyde)
FROM report.SuppliersFullStatistics
WHERE phase_id = @phaseId



--SELECT @totalBooks AS totalbooks ,@pricedBooks ,@avgPricedBooks ,@totalCost ,@toyde 
