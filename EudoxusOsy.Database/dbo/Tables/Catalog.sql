CREATE TABLE [dbo].[Catalog] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [CatalogType]  INT            NOT NULL,
    [GroupID]      INT            NULL,
    [PhaseID]      INT            NULL,
    [OriginalPhaseID]      INT            NULL,
    [BookID]       INT            NOT NULL,
    [SupplierID]   INT            NOT NULL,
    [DepartmentID] INT            NOT NULL,
    [DiscountID]   INT            NOT NULL,
    [BookCount]    INT            NOT NULL,
    [Percentage]   DECIMAL (5, 2) NULL,
    [BookPriceID]  INT            NULL,
    [Amount]       DECIMAL (8, 2) NULL,
    [OldCatalogID] INT            NULL,
    [NewCatalogID] INT            NULL,
	[IsBookActive] BIT            NOT NULL DEFAULT 1,
	[HasPendingPriceVerification] BIT NOT NULL DEFAULT 0,
	[HasUnexpectedPriceChange] BIT NOT NULL DEFAULT 0,
    [State]        INT            NOT NULL,
    [Status]       INT            NOT NULL,
    [CreatedAt]    DATETIME2 (7)  NOT NULL,
    [CreatedBy]    NVARCHAR (256) NOT NULL,
    [UpdatedAt]    DATETIME2 (7)  NULL,
    [UpdatedBy]    NVARCHAR (256) NULL,
    CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Catalog_Book] FOREIGN KEY ([BookID]) REFERENCES [dbo].[Book] ([ID]),
    CONSTRAINT [FK_Catalog_BookPrice] FOREIGN KEY ([BookPriceID]) REFERENCES [dbo].[BookPrice] ([ID]),
    CONSTRAINT [FK_Catalog_CatalogGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[CatalogGroup] ([ID]),
    CONSTRAINT [FK_Catalog_Discount] FOREIGN KEY ([DiscountID]) REFERENCES [dbo].[Discount] ([ID]),
    CONSTRAINT [FK_Catalog_NewCatalog] FOREIGN KEY ([NewCatalogID]) REFERENCES [dbo].[Catalog] ([ID]),
    CONSTRAINT [FK_Catalog_OldCatalog] FOREIGN KEY ([OldCatalogID]) REFERENCES [dbo].[Catalog] ([ID]),
    CONSTRAINT [FK_Catalog_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]),
    CONSTRAINT [FK_Catalog_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]), 
    CONSTRAINT [FK_Catalog_OriginalPhase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase]([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_GroupID]
    ON [dbo].[Catalog]([GroupID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_PhaseID]
    ON [dbo].[Catalog]([PhaseID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_BookID]
    ON [dbo].[Catalog]([BookID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_SupplierID]
    ON [dbo].[Catalog]([SupplierID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_BookPriceID]
    ON [dbo].[Catalog]([BookPriceID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_OldCatalogID]
    ON [dbo].[Catalog]([OldCatalogID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Catalog_NewCatalogID]
    ON [dbo].[Catalog]([NewCatalogID] ASC);

