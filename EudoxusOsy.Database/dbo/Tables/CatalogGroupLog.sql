CREATE TABLE [dbo].[CatalogGroupLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [int] NOT NULL,
	[OldState] [int] NULL,
	[NewState] [int] NOT NULL,
	[OldValues] [xml] NULL,
	[NewValues] [xml] NULL,
	[Comments] nvarchar(max) null, 
	[Amount] float null,
	[PdfNotes] nvarchar(500) null,
	[OfficeSlipNumber] int null,
	[OfficeSlipDate] datetime2(7) null,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_CatalogGroupLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[CatalogGroupLog] ADD  CONSTRAINT [DF_CatalogGroupLog_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[CatalogGroupLog]  WITH CHECK ADD  CONSTRAINT [FK_CatalogGroupLog_CatalogGroup] FOREIGN KEY([GroupID])
REFERENCES [dbo].[CatalogGroup] ([ID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CatalogGroupLog] CHECK CONSTRAINT [FK_CatalogGroupLog_CatalogGroup]
GO