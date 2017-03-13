CREATE TABLE [dbo].[BankTransfer] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [PhaseID]       INT            NOT NULL,
    [SupplierID]    INT            NOT NULL,
    [BankID]        INT            NOT NULL,
    [InvoiceNumber] NVARCHAR (50)  NOT NULL,
    [InvoiceDate]   DATETIME2 (7)  NOT NULL,
    [InvoiceValue]  DECIMAL (9, 2) NOT NULL,
    [IsActive]      BIT            NOT NULL,
    [CreatedAt]     DATETIME2 (7)  NOT NULL,
    [CreatedBy]     NVARCHAR (256) NOT NULL,
    [UpdatedAt]     DATETIME2 (7)  NULL,
    [UpdatedBy]     NVARCHAR (256) NULL,
    CONSTRAINT [PK_BankTransfer] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_BankTransfer_Bank] FOREIGN KEY ([BankID]) REFERENCES [dbo].[Bank] ([ID]),
    CONSTRAINT [FK_BankTransfer_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]),
    CONSTRAINT [FK_BankTransfer_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BankTransfer_PhaseID]
    ON [dbo].[BankTransfer]([PhaseID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BankTransfer_SupplierID]
    ON [dbo].[BankTransfer]([SupplierID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BankTransfer_BankID]
    ON [dbo].[BankTransfer]([BankID] ASC);

