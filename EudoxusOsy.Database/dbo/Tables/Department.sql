CREATE TABLE [dbo].[Department] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [InstitutionID]  INT            NOT NULL,
    [SecretaryKpsID] INT            NULL,
    [LibraryKpsID]   INT            NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [IsActive]       BIT            NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Department_Institution] FOREIGN KEY ([InstitutionID]) REFERENCES [dbo].[Institution] ([ID])
);

