CREATE 
PROCEDURE [dbo].[spCompareReceipts] @phaseId INT, @compareSide INT AS
BEGIN

	IF @compareSide = 1 
	BEGIN
		SELECT ar.RegistrationKpsID, ar.BookKpsID , ISNULL(ar.SecretaryKpsID, ar.LibraryKpsID ) AS SecretaryOrLibraryKpsID, ISNULL(ar.BookCount,1) AS BookCount
		FROM AuditReceiptXml ar
		JOIN Book b ON b.BookKpsID = ar.BookKpsID AND b.IsActive = 1
		LEFT JOIN Department sec ON  sec.SecretaryKpsID = ar.SecretaryKpsID AND sec.IsActive = 1
		LEFT JOIN Department lib ON  lib.LibraryKpsID = ar.LibraryKpsID AND lib.IsActive = 1
		LEFT JOIN Receipt r ON r.DepartmentID = ISNULL(sec.ID, lib.ID) AND 
							   r.BookID = b.ID AND 
							   r.BookCount = ISNULL(ar.BookCount,1)	AND 
							   r.PhaseID = @phaseId	AND
							   r.RegistrationKpsID = ar.RegistrationKpsID							   
		WHERE r.ID IS NULL 
		ORDER BY ar.RegistrationKpsID
	END
	ELSE
	BEGIN
		SELECT r.RegistrationKpsID, b.BookKpsID, ISNULL(dep.SecretaryKpsID, dep.LibraryKpsID) AS SecretaryOrLibraryKpsID, r.BookCount
		FROM Receipt r 
		JOIN Book b ON b.ID = r.BookID AND b.IsActive = 1
		JOIN Department dep ON  dep.ID = r.DepartmentID AND dep.IsActive = 1
		LEFT JOIN (
			SELECT ar.RegistrationKpsID, b.ID AS BookID, ISNULL(sec.ID, lib.ID) AS DepartmentID, ISNULL(ar.BookCount,1) AS BookCount, ar.Reason			
			FROM AuditReceiptXml ar
			JOIN Book b ON b.BookKpsID = ar.BookKpsID AND b.IsActive = 1
			LEFT JOIN Department sec ON  sec.SecretaryKpsID = ar.SecretaryKpsID AND sec.IsActive = 1
			LEFT JOIN Department lib ON  lib.LibraryKpsID = ar.LibraryKpsID AND lib.IsActive = 1
		) AS ar ON r.DepartmentID = ar.DepartmentID AND 
				   r.BookID = ar.BookID AND 
				   r.BookCount = ar.BookCount AND
				   r.RegistrationKpsID = ar.RegistrationKpsID				   
		WHERE r.PhaseID = @phaseId AND 
				ar.RegistrationKpsID IS NULL AND 
				r.State = 1
		ORDER BY r.RegistrationKpsID
	END	
END