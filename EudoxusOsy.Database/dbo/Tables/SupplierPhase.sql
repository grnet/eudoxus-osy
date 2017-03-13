CREATE TABLE [dbo].[SupplierPhase] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [SupplierID] INT            NOT NULL,
    [PhaseID]    INT            NOT NULL,
    [TotalDebt]  FLOAT (53)     NOT NULL,
    [IsActive]   BIT            NOT NULL,
    [CreatedAt]  DATETIME2 (7)  NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [UpdatedAt]  DATETIME2 (7)  NULL,
    [UpdatedBy]  NVARCHAR (256) NULL,
    CONSTRAINT [PK_SupplierPhases] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SupplierPhase_Phase] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[Phase] ([ID]),
    CONSTRAINT [FK_SupplierPhase_Supplier] FOREIGN KEY ([SupplierID]) REFERENCES [dbo].[Supplier] ([ID])
);

