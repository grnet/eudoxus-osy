CREATE TABLE [dbo].[Institution] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL,
    [IsActive] BIT            NOT NULL,
    CONSTRAINT [PK_Institution] PRIMARY KEY CLUSTERED ([ID] ASC)
);

