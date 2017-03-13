CREATE TABLE [dbo].[BookSupplier] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [BookID]     INT            NOT NULL,
    [SupplierID] INT            NOT NULL,
    [Percentage] DECIMAL (5, 2) NOT NULL,
    [Year]       INT            NOT NULL,
    [IsActive]   BIT            NOT NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [UpdatedAt]  DATETIME2 (7)  NULL,
    [UpdatedBy]  NVARCHAR (256) NULL,
    CONSTRAINT [PK_BookSupplier] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BookSupplier_Book] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Book] ([ID]),
    CONSTRAINT [FK_BookSupplier_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BookSupplier_SupplierID]
    ON [dbo].[BookSupplier]([SupplierID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BookSupplier_BookID]
    ON [dbo].[BookSupplier]([BookID] ASC);

