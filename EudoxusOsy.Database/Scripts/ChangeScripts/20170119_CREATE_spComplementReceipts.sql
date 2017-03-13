CREATE
PROCEDURE [dbo].[spComplementReceipts] @phaseId INT AS
BEGIN
BEGIN TRY
		UPDATE r SET state = 2, UpdatedBy = 'sysadmin', UpdatedAt = GETDATE()
		FROM Receipt r 
		LEFT JOIN (
			SELECT ar.KpsRegistrationID, b.ID AS BookID, ISNULL(sec.ID, lib.ID) AS DepartmentID, ISNULL(ar.Amount,1) AS BookCount			
			FROM ReceiptsOnly ar
			JOIN Book b ON b.BookKpsID = ar.BookKpsID AND b.IsActive = 1
			LEFT JOIN Department sec ON  sec.SecretaryKpsID = ar.DeparmtentKpsID AND sec.IsActive = 1
			LEFT JOIN Department lib ON  lib.LibraryKpsID = ar.DeparmtentKpsID AND lib.IsActive = 1
		) AS ar ON r.RegistrationKpsID = ar.KpsRegistrationID 	
		WHERE r.PhaseID = @phaseId AND 
				r.State = 1

		INSERT INTO Receipt(PhaseID, BookID, DepartmentID, BookCount, State, Status, RegistrationKpsID, CreatedAt, CreatedBy)
		SELECT @phaseId,  b.ID AS BookID, ISNULL(sec.ID, lib.ID) AS DepartmentID, ISNULL(ar.Amount,1) ,1 , 0,  ar.KpsRegistrationID,
		 GETDATE(), 'sysadmin'
		FROM KpsOnly ar
		JOIN Book b ON b.BookKpsID = ar.BookKpsID AND b.IsActive = 1
		LEFT JOIN Department sec ON  sec.SecretaryKpsID = ar.DepartmentKpsID AND sec.IsActive = 1
		LEFT JOIN Department lib ON  lib.LibraryKpsID = ar.DepartmentKpsID AND lib.IsActive = 1

		INSERT INTO imis_Log(Date, Thread, Level, Logger, Message, Exception)
			VALUES (GETDATE(), null, 'INFO', '[spComplementReceipts]', 'spComplementReceipts has successfully run', null)

	END TRY
	BEGIN CATCH
		DECLARE @severity int = error_severity()
		DECLARE @level nvarchar(20)

		Select @level = CASE when @Severity <= 10 THEN 'INFO' ELSE 'ERROR' END

		INSERT INTO imis_Log(Date, Thread, Level, Logger, Message, Exception)
			VALUES (GETDATE(), null, @level, '[spComplementReceipts]', ERROR_MESSAGE(), ERROR_STATE())
	END CATCH
END

--EXEC [dbo].[spComplementReceipts] 12

