CREATE TABLE [dbo].[TempArchive]
(
	[ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[KPSArchiveID] INT NOT NULL,
	[BookID] INT NOT NULL, 
	[Year] INT NOT NULL, 
	[Price] DECIMAL(6, 2) NULL, 
	[SuggestedPrice] DECIMAL(6, 2) NOT NULL, 
	[Fek] NVARCHAR(50) NULL, 
	[LastUpdate] NVARCHAR(20) NOT NULL, 
	[DecisionNumber] NVARCHAR(50) NULL, 
	[PriceComments] NVARCHAR(500) NULL, 
	[CheckedPrice] BIT NULL, 
	CONSTRAINT [AK_TempArchive_BookID_Year] UNIQUE (BookID, Year) 
)

GO

CREATE INDEX [IX_TempArchive_KPSArchiveID] ON [dbo].[Archive] ([KPSArchiveID])

GO

CREATE INDEX [IX_TempArchive_BookID] ON [dbo].[Archive] ([BookKpsID])
