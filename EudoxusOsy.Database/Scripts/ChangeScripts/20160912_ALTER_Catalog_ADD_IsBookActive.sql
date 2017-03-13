ALTER TABLE Catalog ADD IsBookActive BIT NOT NULL DEFAULT 1

GO

UPDATE c
SET c.IsBookActive = b.IsActive
FROM Catalog c
	INNER JOIN Book b ON c.BookID = b.ID