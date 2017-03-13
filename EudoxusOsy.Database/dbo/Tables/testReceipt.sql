CREATE TABLE [dbo].[testReceipt] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [PhaseID]         INT            NOT NULL,
    [BookID]          INT            NOT NULL,
    [DepartmentID]    INT            NULL,
    [BookCount]       INT            NOT NULL,
    [PreviousPhaseID] INT            NULL,
    [State]           INT            NOT NULL,
    [Status]          INT            NOT NULL,
    [CreatedAt]       DATETIME2 (7)  NOT NULL,
    [CreatedBy]       NVARCHAR (256) NOT NULL,
    [UpdatedAt]       DATETIME2 (7)  NULL,
    [UpdatedBy]       NVARCHAR (256) NULL
);

