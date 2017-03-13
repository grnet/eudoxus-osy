CREATE TABLE [dbo].[CatalogLog] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [CatalogID]        INT            NULL,
    [ReverseCatalogID] INT            NULL,
    [Action]           INT            NOT NULL,
    [Operations]       NVARCHAR (1000) NOT NULL,
    [CreatedAt]        DATETIME2 (7)  NOT NULL,
    [CreatedBy]        NVARCHAR (256) NOT NULL,
    [UpdatedAt]        DATETIME2 (7)  NULL,
    [UpdatedBy]        NVARCHAR (256) NULL,
    CONSTRAINT [PK_CatalogLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CatalogLog_Catalog] FOREIGN KEY ([CatalogID]) REFERENCES [dbo].[Catalog] ([ID]),
    CONSTRAINT [FK_CatalogLog_ReverseCatalog] FOREIGN KEY ([ReverseCatalogID]) REFERENCES [dbo].[Catalog] ([ID])
);

