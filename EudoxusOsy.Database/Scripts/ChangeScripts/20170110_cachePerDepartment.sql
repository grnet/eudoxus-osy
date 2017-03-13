ALTER 
PROCEDURE report.spCachePerDepartment @phaseId INT AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
	CREATE TABLE #tmp_debtPerDepartment
	(
		department_id INT,
		debt FLOAT,
		phase_id INT
	)

	INSERT INTO #tmp_debtPerDepartment(department_id, debt, phase_id)
	SELECT r.DepartmentID, SUM(bp.Price) as debt, r.PhaseID
	FROM Phase p 
	JOIN BookPrice bp ON bp.Year = p.Year
	JOIN (
		 SELECT BookID, State, Status, DepartmentID, PhaseID
		 FROM Receipt
		 WHERE PhaseID = @phaseId
			AND state = 1 AND Status <> 2
	 ) as r ON r.BookID = bp.BookID
	WHERE bp.status = 1 AND p.ID = @phaseId AND p.IsActive = 1
	GROUP BY r.DepartmentID, r.PhaseID

	DELETE FROM report.ViewStatisticsPerDepartment WHERE phase_id = @phaseId

	INSERT INTO report.ViewStatisticsPerDepartment (department_id, debt, phase_id)
    SELECT department_id, debt, phase_id
    FROM #tmp_debtPerDepartment

	DROP TABLE #tmp_debtPerDepartment

END
GO

--EXEC report.spCachePerDepartment 10