CREATE TABLE [dbo].[imis_Log] (
    [ID]        INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Date]      DATETIME       NOT NULL,
    [Thread]    VARCHAR (255)  NULL,
    [Level]     VARCHAR (20)   NULL,
    [Logger]    VARCHAR (255)  NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [Exception] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_imis_Log] PRIMARY KEY CLUSTERED ([ID] ASC)
);

