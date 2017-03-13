CREATE TABLE [dbo].[CatalogGroup] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [PhaseID]        INT            NOT NULL,
    [SupplierID]     INT            NOT NULL,
    [SupplierIBANID] INT            NULL,
    [InstitutionID]  INT            NOT NULL,
    [DeductionID]    INT            NULL,
    [Vat]            DECIMAL (7, 2) NULL,
    [IsTransfered]   BIT            NOT NULL,
    [BankID]         INT            NULL,
    [IsLocked]       BIT            NOT NULL,
    [State]          INT            NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [Comments]       NVARCHAR (MAX) NULL,
	[ReversalCount]	INT NULL,
    [CreatedAt]      DATETIME2 (7)  NOT NULL,
    [CreatedBy]      NVARCHAR (256) NOT NULL,
    [UpdatedAt]      DATETIME2 (7)  NULL,
    [UpdatedBy]      NVARCHAR (256) NULL,
    CONSTRAINT [PK_CatalogGroup] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CatalogGroup_Bank] FOREIGN KEY ([BankID]) REFERENCES [dbo].[Bank] ([ID]),
    CONSTRAINT [FK_CatalogGroup_Deduction] FOREIGN KEY ([DeductionID]) REFERENCES [dbo].[Deduction] ([ID]),
    CONSTRAINT [FK_CatalogGroup_Institution] FOREIGN KEY ([InstitutionID]) REFERENCES [dbo].[Institution] ([ID]),
    CONSTRAINT [FK_CatalogGroup_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]),
    CONSTRAINT [FK_CatalogGroup_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID]),
    CONSTRAINT [FK_CatalogGroup_SupplierIBAN] FOREIGN KEY ([SupplierIBANID]) REFERENCES [dbo].[SupplierIBAN] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CatalogGroup_SupplierID]
    ON [dbo].[CatalogGroup]([SupplierID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CatalogGroup_SupplierIBANID]
    ON [dbo].[CatalogGroup]([SupplierIBANID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CatalogGroup_InstitutionID]
    ON [dbo].[CatalogGroup]([InstitutionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CatalogGroup_DeductionID]
    ON [dbo].[CatalogGroup]([DeductionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CatalogGroup_BankID]
    ON [dbo].[CatalogGroup]([BankID] ASC);

