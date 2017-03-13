CREATE TABLE [dbo].[Receipt](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PhaseID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[DepartmentID] [int] NULL,
	[BookCount] [int] NOT NULL,
	[PreviousPhaseID] [int] NULL,
	[State] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[RegistrationKpsID] [int] NULL,
	[SentByKpsAt] [datetime] NULL,
	[ReceivedAt] [datetime] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](256) NULL,
 CONSTRAINT [PK_Receipt] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Book] FOREIGN KEY([BookID])
REFERENCES [dbo].[Book] ([ID])
GO

ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Book]
GO

ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Department] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([ID])
GO

ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Department]
GO

ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_Phase] FOREIGN KEY([PhaseID])
REFERENCES [dbo].[Phase] ([ID])
GO

ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_Phase]
GO

ALTER TABLE [dbo].[Receipt]  WITH CHECK ADD  CONSTRAINT [FK_Receipt_PreviousPhase] FOREIGN KEY([PreviousPhaseID])
REFERENCES [dbo].[Phase] ([ID])
GO

ALTER TABLE [dbo].[Receipt] CHECK CONSTRAINT [FK_Receipt_PreviousPhase]
GO


