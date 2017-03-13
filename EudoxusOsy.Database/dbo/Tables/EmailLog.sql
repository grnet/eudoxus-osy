CREATE TABLE [dbo].[EmailLog] (
    [ID]                 INT             IDENTITY (1, 1) NOT NULL,
    [ReporterID]         INT             NULL,
    [Type]               INT             NOT NULL,
    [SenderEmailAddress] NVARCHAR (256)  NOT NULL,
    [EmailAddress]       NVARCHAR (256)  NOT NULL,
    [CCedEmailAddresses] NVARCHAR (1024) NULL,
    [Subject]            NVARCHAR (MAX)  NOT NULL,
    [Body]               NVARCHAR (MAX)  NOT NULL,
    [SentAt]             DATETIME2 (7)   NULL,
    [DeliveryStatus]     INT             NOT NULL,
    [EmailEntityType]    INT             DEFAULT ((0)) NOT NULL,
    [EntityID]           INT             NULL,
    [LastAttemptAt]      DATETIME2 (7)   NULL,
    CONSTRAINT [PK_EmailLog] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EmailLog_Reporter] FOREIGN KEY ([ReporterID]) REFERENCES [dbo].[Reporter] ([ID])
);

