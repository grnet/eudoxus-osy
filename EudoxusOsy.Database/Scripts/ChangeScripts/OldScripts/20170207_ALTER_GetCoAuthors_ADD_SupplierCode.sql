ALTER PROCEDURE [dbo].[GetCoAuthors]
        @myPhaseID int
AS 
Begin

DECLARE @phaseYear int

SELECT @phaseYear = YEAR from Phase where ID = @myPhaseID

SELECT DISTINCT
	supPerc.SupplierKpsID, 
	supPerc.Name, 
	supPerc.percentage, 
	CASE WHEN supPerc.supplierType = 1 then 'REGULAR' ELSE 'SELF' END as supplierType, 
	bookData.BookKpsID, 
	bookData.Title, 
    bookData.Author, 
	bookData.isbn, 
	bookData.SupplierCode,
	bookData.Publisher as Book_publisherInBook 
FROM (
        SELECT sups.SupplierKpsID, innerq.percentage, 
		sups.supplierType, innerq.BookID, sups.Name
        from (
                SELECT orig.SupplierID, orig.percentage, orig.BookID
                FROM bookSupplier AS orig
                INNER JOIN (
                    SELECT BookID, Year
                    FROM bookSupplier
                    WHERE (@myPhaseID = 0 OR year = @phaseYear)
                    AND IsActive = 1
                    GROUP BY BookID, Year
                    HAVING count( BookID ) >1
                )dup 
                    ON orig.BookID = dup.BookID and orig.Year = dup.Year
            WHERE (@myPhaseID = 0 OR orig.year = @phaseYear) 
                AND orig.IsActive = 1 ) as innerq 
        LEFT JOIN supplier as sups ON innerq.SupplierID = sups.ID) as  supPerc
        LEFT JOIN book as bookData ON supPerc.BookID = bookData.ID

END
GO


