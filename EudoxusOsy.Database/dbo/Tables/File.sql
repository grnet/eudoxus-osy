CREATE TABLE [dbo].[File] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [FileName]  NVARCHAR (500) NOT NULL,
    [PathName]  NVARCHAR (500) NOT NULL,
    [IsActive]  BIT            NOT NULL,
	[PdfNotes]  NVARCHAR(500) NULL,
    [CreatedAt] DATETIME2 (7)  NOT NULL,
    [CreatedBy] NVARCHAR (256) NOT NULL,
    [UpdatedAt] DATETIME2 (7)  NULL,
    [UpdatedBy] NVARCHAR (256) NULL,
    CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([ID] ASC)
);

