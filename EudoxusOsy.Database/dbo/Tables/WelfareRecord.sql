CREATE TABLE [dbo].[WelfareRecord] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [OrderIndex]       INT            NOT NULL,
    [Year]             INT            NOT NULL,
    [State]            INT            NOT NULL,
    [MinistryDecision] NVARCHAR (50)  NULL,
    [TotalSample]      INT            NOT NULL,
    [PublishedAt]      DATETIME2 (7)  NULL,
    [IsActive]         BIT            NOT NULL,
    [CreatedAt]        DATETIME2 (7)  NOT NULL,
    [CreatedBy]        NVARCHAR (256) NOT NULL,
    [UpdatedAt]        DATETIME2 (7)  NULL,
    [UpdatedBy]        NVARCHAR (256) NULL,
    CONSTRAINT [PK_WelfareRecord] PRIMARY KEY CLUSTERED ([ID] ASC)
);

