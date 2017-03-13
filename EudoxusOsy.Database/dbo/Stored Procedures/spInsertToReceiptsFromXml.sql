CREATE 
PROCEDURE [dbo].[spInsertToReceiptsFromXml] @phaseId INT AS
BEGIN

	INSERT INTO [dbo].[Receipt]
		   ([PhaseID]
		   ,[BookID]
		   ,[DepartmentID]
		   ,[BookCount]           
		   ,[State]
		   ,[Status]
		   ,[RegistrationKpsID]                      
		   ,[CreatedAt]
		   ,[CreatedBy]
		   )
	SELECT @phaseId,
			b.ID,
			ISNULL(dep.ID, d.ID),
			ISNULL(ar.BookCount,1),
			CASE WHEN Reason = 'DELIVERED' THEN 1 WHEN Reason IN ('CANCELLED', 'CANCELED') THEN 2 ELSE -100 END,						
			0,
			ar.[RegistrationKpsID],
			GETDATE(),
			'sysadmin'		
	FROM AuditReceiptXml ar
	JOIN Book b ON b.BookKpsID = ar.BookKpsID
	LEFT JOIN Department d ON d.LibraryKpsID = ar.LibraryKpsID
	LEFT JOIN Department dep ON dep.SecretaryKpsID = ar.SecretaryKpsID

END


