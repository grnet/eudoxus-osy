CREATE TABLE [dbo].[SupplierIBAN] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [SupplierID] INT            NOT NULL,
    [IBAN]       NVARCHAR (34)  NOT NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_SupplierIBAN] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierIBAN_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

