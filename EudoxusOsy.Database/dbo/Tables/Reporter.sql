CREATE TABLE [dbo].[Reporter] (
    [ID] [int] NOT NULL,
	[ReporterType] [int] NOT NULL,
	[ContactName] [nvarchar](200) NULL,
	[ContactPhone] [nvarchar](100) NULL,
	[ContactMobilePhone] [nvarchar](100) NULL,
	[ContactEmail] [nvarchar](256) NULL,
	[AuthorizationType] [int] NULL,
	[Description] nvarchar(500) NULL,
	[Email] [nvarchar](256) NULL,
	[Username] nvarchar(256) NULL,
	[VerificationStatus] [int] NOT NULL,
	[IsActivated] BIT CONSTRAINT [DF_Reporter_IsApproved] DEFAULT ((0)) NOT NULL,	
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](256) NULL,
    CONSTRAINT [PK_Reporter] PRIMARY KEY CLUSTERED ([ID] ASC)
);

