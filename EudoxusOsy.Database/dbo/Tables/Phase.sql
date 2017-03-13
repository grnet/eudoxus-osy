CREATE TABLE [dbo].[Phase] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [Year]              INT             NOT NULL,
    [StartDate]         DATETIME2 (7)   NOT NULL,
    [EndDate]           DATETIME2 (7)   NULL,
    [PhaseAmount]       FLOAT (53)      NOT NULL,
    [TotalDebt]         DECIMAL (10, 2) NULL,
    [RequestsStartDate] DATETIME2 (7)   NULL,
    [RequestsEndDate]   DATETIME2 (7)   NULL,
    [IsActive]          BIT             NOT NULL,
	[CatalogsCreated]	BIT				NOT NULL DEFAULT 0,
    [CreatedAt]         DATETIME2 (7)   NOT NULL,
    [CreatedBy]         NVARCHAR (256)  NOT NULL,
    [UpdatedAt]         DATETIME2 (7)   NULL,
    [UpdatedBy]         NVARCHAR (256)  NULL,
    CONSTRAINT [PK_Phase] PRIMARY KEY CLUSTERED ([ID] ASC)
);

