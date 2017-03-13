CREATE TABLE [dbo].[BookBlackList] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [BookID]             INT            NOT NULL,
    [BookPriceRequestID] INT            NULL,
    [Comments]           NVARCHAR (MAX) NULL,
    [Status]             INT            NOT NULL,
    [CreatedAt]          DATETIME2 (7)  NOT NULL,
    [CreatedBy]          NVARCHAR (256) NOT NULL,
    [UpdatedAt]          DATETIME2 (7)  NULL,
    [UpdatedBy]          NVARCHAR (256) NULL,
    CONSTRAINT [PK_BookBlackList] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BookBlackList_Book] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Book] ([ID]),
    CONSTRAINT [FK_BookBlackList_BookPriceRequest] FOREIGN KEY ([BookPriceRequestID]) REFERENCES [dbo].[BookPriceRequest] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BookBlackList_BookID]
    ON [dbo].[BookBlackList]([BookID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BookBlackList_BookPriceRequestID]
    ON [dbo].[BookBlackList]([BookPriceRequestID] ASC);

