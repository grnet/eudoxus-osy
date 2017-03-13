ALTER TABLE CatalogLog
ADD NewOperations nvarchar(1000)

GO

Update Cataloglog
SET NewOperations = Operations
GO

ALTER TABLE CatalogLog 
DROP Column Operations
GO

ALTER Table CatalogLog 
ADD Operations nvarchar(1000)
GO

Update CatalogLog 
SET Operations = NewOperations
GO

ALTER TABLE CatalogLog 
DROP Column NewOperations
GO

ALTER TABLE CatalogLog
ALTER Column CatalogID int null

GO