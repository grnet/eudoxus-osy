CREATE TABLE [dbo].[SmsLog] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [ReporterID]     INT            NULL,
    [Type]           INT            NOT NULL,
    [SendID]         NVARCHAR (200) NOT NULL,
    [ReporterNumber] NVARCHAR (12)  NOT NULL,
    [Msg]            NVARCHAR (200) NOT NULL,
    [FieldValues]    NVARCHAR (500) NULL,
    [SentAt]         DATETIME2 (7)  NULL,
    [DeliveryStatus] INT            NOT NULL,
    [EntityID]       INT            NULL,
    [LastAttemptAt]  DATETIME2 (7)  NULL,
    CONSTRAINT [PK_SmsLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SmsLog_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID])
);

