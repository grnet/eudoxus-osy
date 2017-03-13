CREATE TABLE [dbo].[PaymentOrder] (
    [ID]                     INT            IDENTITY (1, 1) NOT NULL,
    [GroupID]                INT            NOT NULL,
    [FileID]                 INT            NULL,
    [PreviousPaymentOrderID] INT            NULL,
    [OfficeSlipNumber]       INT            NOT NULL,
    [OfficeSlipDate]         DATETIME2            NULL,
    [Amount]                 FLOAT (53)     NOT NULL,
    [Comments]               NVARCHAR (MAX) NULL,
    [State]                  INT            NOT NULL,
    [IsActive]               BIT            NULL,
    [CreatedAt]              DATETIME2 (7)  NULL,
    [CreatedBy]              NVARCHAR (256) NULL,
    [UpdatedAt]              DATETIME2 (7)  NULL,
    [UpdatedBy]              NVARCHAR (256) NULL,
    CONSTRAINT [PK_PaymentOrder] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_PaymentOrder_CatalogGroup] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[CatalogGroup] ([ID]),
    CONSTRAINT [FK_PaymentOrder_File] FOREIGN KEY ([FileID]) REFERENCES [dbo].[File] ([ID]),
    CONSTRAINT [FK_PaymentOrder_PreviousPaymentOrder] FOREIGN KEY ([PreviousPaymentOrderID]) REFERENCES [dbo].[PaymentOrder] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentOrder_GroupID]
    ON [dbo].[PaymentOrder]([GroupID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentOrder_FileID]
    ON [dbo].[PaymentOrder]([FileID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PaymentOrder_PreviousPaymentIOrderID]
    ON [dbo].[PaymentOrder]([PreviousPaymentOrderID] ASC);

