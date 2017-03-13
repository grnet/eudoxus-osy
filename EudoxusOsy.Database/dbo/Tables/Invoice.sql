CREATE TABLE [dbo].[Invoice] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]       INT            NOT NULL,
    [InvoiceNumber] NVARCHAR (100) NOT NULL,
    [InvoiceDate]   DATETIME2 (7)  NOT NULL,
    [InvoiceValue]  DECIMAL (9, 2) NOT NULL,
    [IsActive]      BIT            NOT NULL,
    [CreatedAt]     DATETIME2 (7)  NOT NULL,
    [CreatedBy]     NVARCHAR (256) NOT NULL,
    [UpdatedAt]     DATETIME2 (7)  NULL,
    [UpdatedBy]     NVARCHAR (256) NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Invoice_CatalogGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[CatalogGroup] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Invoice_GroupID]
    ON [dbo].[Invoice]([GroupID] ASC);

