CREATE TABLE [dbo].[Book] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [BookKpsID]             INT            NOT NULL,
    [BookType]              INT            NOT NULL,
    [Title]                 NVARCHAR (500) NOT NULL,
    [Subtitle]              NVARCHAR (500) NULL,
    [Author]                NVARCHAR (MAX) NULL,
    [Publisher]             NVARCHAR (500) NULL,
    [Pages]                 INT            NOT NULL,
    [ISBN]                  NVARCHAR (100) NULL,
    [Comments]              NVARCHAR (MAX) NULL,
	[FirstRegistrationYear] INT            NULL,
	[SupplierCode] INT NULL,
    [IsActive]              BIT            NOT NULL,
	[PendingCommitteePriceVerification] BIT  NULL,
	[HasUnexpectedPriceChangePhaseID] int  NULL,
    [CreatedAt]             DATETIME2 (7)  NOT NULL,
    [CreatedBy]             NVARCHAR (256) NOT NULL,
    [UpdatedAt]             DATETIME2 (7)  NULL,
    [UpdatedBy]             NVARCHAR (256) NULL,         
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Book_BookKpsID]
    ON [dbo].[Book]([BookKpsID] ASC);

