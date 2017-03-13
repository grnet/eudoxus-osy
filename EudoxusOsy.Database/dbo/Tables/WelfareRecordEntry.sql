CREATE TABLE [dbo].[WelfareRecordEntry] (
    [ID]                        INT            IDENTITY (1, 1) NOT NULL,
    [WelfareRecordID]           INT            NOT NULL,
    [BookPriceRequestID]        INT            NOT NULL,
    [IsSample]                  BIT            NOT NULL,
    [IsSentSample]              BIT            NULL,
    [IsSentApproved]            BIT            NULL,
    [IsForced]                  BIT            NOT NULL,
    [IsLocked]                  BIT            NOT NULL,
    [FromObjection]             BIT            NOT NULL,
    [State]                     INT            NOT NULL,
    [PriceModificationReasonID] INT            NULL,
    [PriceRejectionReasonID]    INT            NULL,
    [CategoryNum]               NVARCHAR (256) NULL,
    [CategoryDescription]       NVARCHAR (256) NULL,
    [IsActive]                  BIT            NOT NULL,
    [CreatedAt]                 DATETIME2 (7)  NOT NULL,
    [CreatedBy]                 NVARCHAR (256) NOT NULL,
    [UpdatedAt]                 DATETIME2 (7)  NULL,
    [UpdatedBy]                 NVARCHAR (256) NULL,
    CONSTRAINT [PK_WelfareRecordEntry] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_WelfareRecordEntry_BookPriceRequest] FOREIGN KEY ([BookPriceRequestID]) REFERENCES [dbo].[BookPriceRequest] ([ID]),
    CONSTRAINT [FK_WelfareRecordEntry_PriceModificationReason] FOREIGN KEY ([PriceModificationReasonID]) REFERENCES [dbo].[PriceModificationReason] ([ID]),
    CONSTRAINT [FK_WelfareRecordEntry_PriceRejectionReason] FOREIGN KEY ([PriceRejectionReasonID]) REFERENCES [dbo].[PriceRejectionReason] ([ID]),
    CONSTRAINT [FK_WelfareRecordEntry_WelfareRecord] FOREIGN KEY ([WelfareRecordID]) REFERENCES [dbo].[WelfareRecord] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX_WelfareRecordEntry_WelfareRecordID]
    ON [dbo].[WelfareRecordEntry]([WelfareRecordID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WelfareRecordEntry_BookPriceRequestID]
    ON [dbo].[WelfareRecordEntry]([BookPriceRequestID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WelfareRecordEntry_PriceModificationReasonID]
    ON [dbo].[WelfareRecordEntry]([PriceModificationReasonID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_WelfareRecordEntry_PriceRejectionReasonID]
    ON [dbo].[WelfareRecordEntry]([PriceRejectionReasonID] ASC);

