ALTER 
PROCEDURE report.spCacheStatsPerDepartment @phaseId INT AS
BEGIN

	DELETE FROM report.StatsNotPricedPerDepartment WHERE phase_id = @phaseId

	INSERT INTO StatsNotPricedPerDepartment (book_id, times_registered, book_title, book_kpsid, department_id, phase_id)
	SELECT innerquery.BookID, innerquery.times as times_registered, b.Title, b.BookKpsID, innerquery.dep, @phaseId
	FROM (		
		SELECT Receipt.BookID, Receipt.DepartmentID AS dep, COUNT(Receipt.BookID) AS times
		FROM Receipt
		JOIN Book b ON b.ID = Receipt.BookID
		WHERE Receipt.BookID
			NOT IN (
				SELECT bp.BookID
				FROM BookPrice bp
				JOIN Phase p ON p.Year = bp.Year
				WHERE bp.Status = 1
					AND bp.year = p.Year
					AND p.ID = @phaseId
					AND p.IsActive = 1
					)
		AND Receipt.State = 1
		AND Receipt.PhaseID = @phaseId
		GROUP BY BookID, DepartmentID
		) as innerquery		
	JOIN Book b ON b.ID = innerquery.BookID

	DELETE FROM report.StatsPricedPerDepartment WHERE phase_id = @phaseId

	INSERT INTO StatsPricedPerDepartment (book_id, times_registered, book_title, book_kpsid, department_id, phase_id)
	SELECT innerquery.BookID, innerquery.times as times_registered, b.Title, b.BookKpsID, innerquery.dep, @phaseId
	FROM (		
		SELECT Receipt.BookID, Receipt.DepartmentID AS dep, COUNT(Receipt.BookID) AS times
		FROM Receipt
		JOIN Book b ON b.ID = Receipt.BookID
		WHERE Receipt.BookID
			IN (
				SELECT bp.BookID
				FROM BookPrice bp
				JOIN Phase p ON p.Year = bp.Year
				WHERE bp.Status = 1
					AND bp.year = p.Year
					AND p.ID = @phaseId
					AND p.IsActive = 1
					)
		AND Receipt.State = 1
		AND Receipt.PhaseID = @phaseId
		GROUP BY BookID, DepartmentID
		) as innerquery		
	JOIN Book b ON b.ID = innerquery.BookID

END 
GO

--EXEC report.spCacheStatsPerDepartment 9 10000