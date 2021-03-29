--EXEC [report].[spCacheDepartments] 12

CREATE
PROCEDURE [report].[spCacheDepartments] @phaseId INT AS
BEGIN
	
	--------------------------------------------------------------------------------
	--Find Books with price
	CREATE TABLE #tmp_pricedBooks(bookId INT)

	INSERT INTO #tmp_pricedBooks(bookId)
	SELECT bp.BookID
	FROM BookPrice bp
	JOIN Phase p ON p.Year = bp.Year
	WHERE bp.Status = 1	AND p.IsActive = 1 AND p.ID = @phaseId
	--------------------------------------------------------------------------------
	--Receipt State = 1 -> Delivered
	--Receipt Status = 1 -> Accounted
	--------------------------------------------------------------------------------
	--Priced - Not Priced
	DELETE FROM report.StatsNotPricedPerDepartment WHERE PhaseID = @phaseId
	DELETE FROM report.StatsPricedPerDepartment WHERE PhaseID = @phaseId

	INSERT INTO StatsNotPricedPerDepartment (BookID, TimesRegistered, BookTitle, BookKpsID, DepartmentID, PhaseID)		
	SELECT b.ID, a.times, b.Title, b.BookKpsID, a.DepartmentID, @phaseId
	FROM Book b
	JOIN (
		SELECT r.BookID, COUNT(DISTINCT r.BookID) AS times, DepartmentID
		FROM Receipt r				
		LEFT JOIN #tmp_pricedBooks tmp ON tmp.bookId = r.BookID
		WHERE tmp.bookId IS NULL AND r.State = 1 AND r.PhaseID = @phaseId
		GROUP BY r.BookID, DepartmentID
	) a ON a.bookID = b.ID
		
	INSERT INTO StatsPricedPerDepartment (BookID, TimesRegistered, BookTitle, BookKpsID, DepartmentID, PhaseID)			
	SELECT b.ID, a.times, b.Title, b.BookKpsID, a.DepartmentID, @phaseId
	FROM Book b
	JOIN (
		SELECT r.BookID, COUNT(DISTINCT r.BookID) AS times, DepartmentID
		FROM Receipt r		
		JOIN Book b ON b.ID = r.BookID
		JOIN #tmp_pricedBooks tmp ON tmp.bookId = r.BookID
		WHERE r.State = 1 AND r.PhaseID = @phaseId
		GROUP BY r.BookID, DepartmentID
	) a ON a.bookID = b.ID

	DROP TABLE #tmp_pricedBooks
	--------------------------------------------------------------------------------	
	CREATE TABLE #tmp_sumCalc(DepartmentID INT,InstitutionID INT, SupplierId INT, BookID INT, Amount decimal(10,2))
	
	INSERT INTO #tmp_sumCalc(DepartmentID, SupplierId, BookID, Amount)
	SELECT r.DepartmentID, bs.SupplierID, r.BookID, SUM(r.BookCount)*ROUND(MAX(price*(percentage/100)),2) 
	FROM Phase p 
	JOIN Receipt r ON r.PhaseID = p.ID		
	JOIN BookPrice bp ON bp.BookID = r.BookID AND bp.Year = p.Year	
	JOIN BookSupplier bs ON bs.BookID = r.BookID AND bs.Year = p.Year AND bs.IsActive = 1
	WHERE r.State = 1 AND r.Status <> 2 AND bp.status = 1 AND p.ID = @phaseId AND p.IsActive = 1 
	GROUP BY r.DepartmentID, bs.SupplierID, r.BookID

	--------------------------------------------------------------------------------	
	--TotalSum  
	DELETE FROM report.ViewTotalSum WHERE PhaseID = @phaseId

	INSERT INTO report.ViewTotalSum(PhaseID, totalSum)
    SELECT @phaseId, SUM(Amount)
    FROM #tmp_sumCalc	    

	UPDATE Phase SET TotalDebt = (SELECT totalSum FROM report.ViewTotalSum WHERE PhaseID = @phaseId )
	WHERE ID = @phaseId
	--------------------------------------------------------------------------------	
	--Departments
		
	DELETE FROM report.ViewStatisticsPerDepartment WHERE PhaseID = @phaseId

	INSERT INTO report.ViewStatisticsPerDepartment(DepartmentID, Debt, PhaseID)
	SELECT DepartmentID, SUM(Amount), @phaseId
	FROM #tmp_sumCalc
	GROUP BY DepartmentID

	UPDATE vs SET PricedCount = priced.pricedCount, NotPricedCount = notPriced.notPricedCount, DepName = d.Name, InstName = inst.Name
	FROM report.ViewStatisticsPerDepartment vs
	JOIN Department d ON d.ID = vs.DepartmentID
	JOIN Institution inst ON inst.ID = d.InstitutionID
	LEFT JOIN (
		SELECT COUNT(DISTINCT StatsNotPricedPerDepartment.BookID) as notPricedCount , dep.ID
		FROM report.StatsNotPricedPerDepartment
		JOIN Department dep ON dep.ID = StatsNotPricedPerDepartment.DepartmentID
		WHERE StatsNotPricedPerDepartment.PhaseID = @phaseId AND dep.IsActive = 1
		GROUP BY dep.ID		
	) as notPriced ON notPriced.ID = vs.DepartmentID
	LEFT JOIN (
		SELECT COUNT(DISTINCT StatsPricedPerDepartment.BookID) as pricedCount , dep.ID
		FROM report.StatsPricedPerDepartment
		JOIN Department dep ON dep.ID = StatsPricedPerDepartment.DepartmentID
		WHERE StatsPricedPerDepartment.PhaseID = @phaseId AND dep.IsActive = 1
		GROUP BY dep.ID			
	) as priced ON priced.ID = vs.DepartmentID
	WHERE d.IsActive = 1 AND inst.IsActive = 1 AND vs.PhaseID = @phaseId
	--------------------------------------------------------------------------------	
	--Institutions

	DELETE FROM report.ViewStatisticsPerInstitution WHERE PhaseID = @phaseId

	INSERT INTO report.ViewStatisticsPerInstitution(InstitutionID, Debt, PhaseID, DepartmentCount)
	SELECT dep.InstitutionID, SUM(Amount), @phaseId, COUNT(DISTINCT dep.ID)
	FROM #tmp_sumCalc
	JOIN Department dep ON dep.ID = DepartmentID
	JOIN Institution inst ON inst.ID = dep.InstitutionID
	GROUP BY dep.InstitutionID

	UPDATE vs SET PricedCount = priced.pricedCount, NotPricedCount = notPriced.notPricedCount, InstName = inst.Name
	FROM report.ViewStatisticsPerInstitution vs
	JOIN Institution inst ON inst.ID = vs.InstitutionID
	LEFT JOIN (
			SELECT COUNT(DISTINCT StatsNotPricedPerDepartment.BookID) as notPricedCount , inst.ID AS InstitutionID
			FROM report.StatsNotPricedPerDepartment
			JOIN Department dep ON dep.ID = StatsNotPricedPerDepartment.DepartmentID
			JOIN Institution inst ON inst.ID = dep.InstitutionID
			WHERE StatsNotPricedPerDepartment.PhaseID = @phaseId AND dep.IsActive = 1 AND inst.IsActive = 1
			GROUP BY inst.ID		
		) notPriced ON notPriced.InstitutionID = vs.InstitutionID
		LEFT JOIN (
			SELECT COUNT(DISTINCT StatsPricedPerDepartment.BookID) as pricedCount , inst.ID AS InstitutionID
			FROM report.StatsPricedPerDepartment
			JOIN Department dep ON dep.ID = StatsPricedPerDepartment.DepartmentID
			JOIN Institution inst ON inst.ID = dep.InstitutionID
			WHERE StatsPricedPerDepartment.PhaseID = @phaseId AND dep.IsActive = 1 AND inst.IsActive = 1
			GROUP BY inst.ID			
		) as priced ON priced.InstitutionID = vs.InstitutionID
		WHERE inst.IsActive = 1 AND vs.PhaseID = @phaseId
	--------------------------------------------------------------------------------	
	DROP TABLE #tmp_sumCalc
END