CREATE 
PROCEDURE [report].[spCacheStats] @phaseId INT AS
BEGIN

IF @phaseId < 1 
BEGIN
	SELECT TOP 1 @phaseID = ID FROM PHASE WHERE IsActive = 1 and EndDate is null
END

EXEC report.spCacheDepartments @phaseId

EXEC report.spCacheSupplierStatistics @phaseId
END

GO
