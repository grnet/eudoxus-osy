CREATE TABLE [dbo].[Supplier] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [ReporterID]         INT            NOT NULL,
    [SupplierKpsID]      INT            NOT NULL,
    [SupplierType]       INT            NOT NULL,
    [Name]               NVARCHAR (512) NOT NULL,
    [TradeName]          NVARCHAR (512) NULL,
    [AFM]                NVARCHAR (20)  NOT NULL,	
    [DOY]                NVARCHAR (50)  NOT NULL,
	[PaymentPfoID]		 INT            NULL,
    [PaymentPfo]         NVARCHAR (50)  NULL,
    [HasLogisticBooks]   BIT            NULL,	
    [Email]              NVARCHAR (100) NOT NULL,
    [VerificationStatus] INT            NOT NULL,
    [IsActivated]        BIT            NOT NULL,
    [Status]             INT            NOT NULL,
    [CreatedAt]          DATETIME2 (7)  NOT NULL,
    [CreatedBy]          NVARCHAR (256) NOT NULL,
    [UpdatedAt]          DATETIME2 (7)  NULL,
    [UpdatedBy]          NVARCHAR (256) NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Supplier_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID]),
    CONSTRAINT [FK_Supplier_SupplierDetails] FOREIGN KEY ([SupplierKpsID]) REFERENCES [dbo].[SupplierDetails] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Supplier_ReporterID]
    ON [dbo].[Supplier]([ReporterID] ASC);

