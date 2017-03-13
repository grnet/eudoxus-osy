ALTER 
PROCEDURE report.spCacheStats @phaseId INT AS
BEGIN

IF @phaseId < 1 
BEGIN
	SELECT TOP 1 @phaseId = ID FROM PHASE WHERE IsActive = 1 and EndDate is null
END

EXEC report.spCacheStatsPerDepartment @phaseId

EXEC report.spCacheTotalSum @phaseId

EXEC report.spCachePerDepartment @phaseId

EXEC report.spCachePerInstitution @phaseId

EXEC report.spCacheSupplierStatistics @phaseId
END
GO
