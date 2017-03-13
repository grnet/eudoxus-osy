CREATE 
PROCEDURE report.spCachePerInstitution @phaseId INT AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
	CREATE TABLE #tmp_debtPerInstitution
	(
		institution_id INT,
		debt FLOAT,
		phase_id INT
	)

	INSERT INTO #tmp_debtPerInstitution(institution_id, debt, phase_id)
	SELECT d.InstitutionID, SUM(vd.debt) as debt, vd.phase_id
	FROM report.ViewStatisticsPerDepartment vd
	JOIN Department d ON d.ID = vd.department_id	
	WHERE d.IsActive = 1 AND vd.phase_id = @phaseId
	GROUP BY d.InstitutionID, vd.phase_id
	 
	DELETE FROM report.ViewStatisticsPerInstitution WHERE phase_id = @phaseId

	INSERT INTO report.ViewStatisticsPerInstitution (institution_id, debt, phase_id)
    SELECT institution_id, debt, phase_id
    FROM #tmp_debtPerInstitution

	DROP TABLE #tmp_debtPerInstitution

END
GO

--EXEC report.spCachePerInstitution 10