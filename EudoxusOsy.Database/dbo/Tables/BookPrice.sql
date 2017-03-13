CREATE TABLE [dbo].[BookPrice] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [BookID]    INT            NOT NULL,
    [Price]     DECIMAL (6, 2) NOT NULL,
    [Year]      INT            NOT NULL,
    [FekYear]   INT            NULL,
    [Fek]       NVARCHAR (200) NULL,
    [Status]    INT            NOT NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [CreatedBy] NVARCHAR (256) NOT NULL,
    [UpdatedAt] DATETIME2 (7)  NULL,
    [UpdatedBy] NVARCHAR (256) NULL,
    CONSTRAINT [PK_BookPrice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BookPrice_Book] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Book] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BookPrice_BookID]
    ON [dbo].[BookPrice]([BookID] ASC);

