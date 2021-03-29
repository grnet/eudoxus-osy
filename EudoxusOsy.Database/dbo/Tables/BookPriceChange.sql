CREATE TABLE [dbo].[BookPriceChange]
(
	[ID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [BookID] INT NOT NULL, 
    [SuggestedPrice] DECIMAL(5, 2) NOT NULL, 
    [Price] DECIMAL(5, 2) NULL, 
    [DecisionNumber] NVARCHAR(50) NOT NULL, 
    [PriceComments] NVARCHAR(500) NOT NULL, 
    [PriceChecked] BIT NOT NULL, 
	[Approved] BIT NULL,
	[IsUnexpected] BIT NULL,
	[Year] INT NOT NULL DEFAULT(0), 
    [CreatedAt] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL,     
    CONSTRAINT [FK_BookPriceChanges_Book] FOREIGN KEY ([BookID]) REFERENCES [Book]([ID])
)
