CREATE TABLE [dbo].[PublicFinancialOffice] (
    [ID]     INT            IDENTITY (1, 1) NOT NULL,
    [GgpsID] INT            NOT NULL,
    [Name]   NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_PublicFinancialOffice] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_PublicFinancialOffice_GgpsID]
    ON [dbo].[PublicFinancialOffice]([GgpsID] ASC);

