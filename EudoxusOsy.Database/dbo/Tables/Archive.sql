CREATE TABLE [dbo].[Archive]
(
	[ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[KPSArchiveID] INT NOT NULL,
    [BookKpsID] INT NOT NULL, 
    [Year] INT NOT NULL, 
    [Price] DECIMAL(6, 2) NULL, 
    [SuggestedPrice] DECIMAL(6, 2) NOT NULL, 
	[Fek] NVARCHAR(50) NULL, 
    [LastUpdate] NVARCHAR(20) NOT NULL, 
    [DecisionNumber] NVARCHAR(50) NULL, 
    [PriceComments] NVARCHAR(500) NULL, 
    [CheckedPrice] BIT NULL, 
    [IsActive] BIT NOT NULL DEFAULT(0), 
    CONSTRAINT [AK_Archive_BookKpsID_Year] UNIQUE (BookKpsID, Year) 
)

GO

CREATE INDEX [IX_Archive_KPSArchiveID] ON [dbo].[Archive] ([KPSArchiveID])

GO


CREATE INDEX [IX_Archive_BookKpsID] ON [dbo].[Archive] ([BookKpsID])

GO

CREATE NONCLUSTERED INDEX [IX_Archive_Status] ON [dbo].[Archive]
(
	[IsActive] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
