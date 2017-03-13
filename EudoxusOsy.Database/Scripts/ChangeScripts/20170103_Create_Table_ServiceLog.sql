CREATE TABLE [dbo].[ServiceLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MethodCall] [nvarchar](256) NOT NULL,
	[CalledAt] [datetime2](7) NOT NULL,
	[CallerID] [int] NULL,
	[CalledBy] [nvarchar](256) NULL,
	[ErrorCode] [nvarchar](50) NULL,
	[Success] [bit] NULL,
	[IP] [nvarchar](50) NULL,
	[PostRequest] [xml] NULL,
	[GetParameters] [nvarchar](50) NULL,
 CONSTRAINT [PK_ServiceLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


