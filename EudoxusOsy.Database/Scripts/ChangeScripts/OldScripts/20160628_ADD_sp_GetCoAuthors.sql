
CREATE PROCEDURE [dbo].[GetCoAuthors]
        @myPhaseID int
AS 
Begin

DECLARE @phaseYear int

SELECT @phaseYear = YEAR from Phase where ID = @myPhaseID

SELECT supPerc.SupplierKpsID, supPerc.Name, supPerc.percentage, 
supPerc.supplierType, bookData.BookKpsID, bookData.Title, 
        bookData.Author, bookData.isbn, bookData.Publisher as Book_publisherInBook 
FROM (
        SELECT sups.SupplierKpsID, innerq.percentage, 
		sups.supplierType, innerq.BookID, sups.Name
        from (
                SELECT orig.SupplierID, orig.percentage, orig.BookID
                FROM bookSupplier AS orig
                INNER JOIN (
                    SELECT BookID
                    FROM bookSupplier
                    WHERE year = @phaseYear
                    AND IsActive = 1
                    GROUP BY BookID
                    HAVING count( BookID ) >1
                )dup 
                    ON orig.BookID = dup.BookID 
            WHERE orig.year = @phaseYear 
                AND orig.IsActive = 1 ) as innerq 
        LEFT JOIN supplier as sups ON innerq.SupplierID = sups.ID) as  supPerc
        LEFT JOIN book as bookData ON supPerc.BookID = bookData.ID

END