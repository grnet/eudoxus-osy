CREATE TABLE [dbo].[LockedCatalogGroup] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]   INT            NOT NULL,
    [Comments]  NVARCHAR (MAX) NULL,
    [IsLocked]  BIT            NOT NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [CreatedBy] NVARCHAR (256) NOT NULL,
    [UpdatedAt] DATETIME2 (7)  NULL,
    [UpdatedBy] NVARCHAR (256) NULL,
    CONSTRAINT [PK_LockedCatalogGroup] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LockedCatalogGroup_CatalogGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[CatalogGroup] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_LockedCatalogGroup_GroupID]
    ON [dbo].[LockedCatalogGroup]([GroupID] ASC);

