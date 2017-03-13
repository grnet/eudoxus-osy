CREATE TABLE [dbo].[QueueEntry] (
    [ID]                 INT           IDENTITY (1, 1) NOT NULL,
    [NumberOfRetries]    INT           NOT NULL,
    [MaxNumberOfRetries] INT           NULL,
    [RetryInterval]      INT           NULL,
    [QueueEntryType]     INT           NOT NULL,
    [QueueEntryStatus]   INT           NOT NULL,
    [QueueEntryPriority] INT           CONSTRAINT [DF_QueueEntry_QueueEntryPriority] DEFAULT ((1)) NOT NULL,
    [QueueDataXml]       XML           NOT NULL,
    [LastAttemptAt]      DATETIME2 (7) NULL,
    [RetryData]          XML           NULL,
    CONSTRAINT [PK_QueueEntry] PRIMARY KEY CLUSTERED ([ID] ASC)
);

