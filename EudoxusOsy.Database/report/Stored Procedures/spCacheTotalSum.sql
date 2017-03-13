CREATE 
PROCEDURE report.spCacheTotalSum @phaseId INT AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
/*
όταν η φάση έχει κλείσει θα πρέπει το συνολικό ποσό να τραβιέται από τη βάση με
select sum(price)
from catalogs
where phase_id=....
and status='ACTIVE'

τρέχει σε 0.1sec
*/
	CREATE TABLE #tmp_totalSum
	(
		totalSum float,
		phase_id INT    
	)

	INSERT INTO #tmp_totalSum(totalSum, phase_id)
    SELECT SUM(price), p.ID
    FROM Phase p
	JOIN Receipt r ON r.PhaseID = p.ID
	JOIN BookPrice bp ON bp.BookID = r.BookID
	WHERE p.ID = @phaseId AND
		  bp.Year = p.Year AND -- FEK YEAR ???
		  bp.Status = 1 AND
		  p.IsActive = 1 AND
		  r.State = 1                                        
    GROUP BY p.ID

	DELETE FROM report.ViewTotalSum WHERE phase_id = @phaseId

	INSERT INTO report.ViewTotalSum(phase_id, totalSum)
    SELECT phase_id, totalSum
    FROM #tmp_totalSum

	DROP TABLE #tmp_totalSum

	UPDATE Phase SET 
	TotalDebt =
		 ( SELECT totalSum
			FROM report.ViewTotalSum
			WHERE phase_id = @phaseId	 
		 )
	WHERE ID = @phaseId
END
GO

--EXEC dbo.spCacheTotalSum 10