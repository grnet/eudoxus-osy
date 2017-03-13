CREATE TABLE [dbo].[AuditReceipt](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationKpsID] [bigint] NOT NULL,
	[KpsBookID] [bigint] NOT NULL,
	[ReceivedAt] [datetime2](7) NOT NULL,
	[SecreteriatKpsID] [bigint] NOT NULL,
	[SentByKpsAt] [datetime2](7) NOT NULL,
	[Reason] [int] NOT NULL DEFAULT ((1)),
	[Amount] [int] NOT NULL DEFAULT ((0)),
	[Request] [xml] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


