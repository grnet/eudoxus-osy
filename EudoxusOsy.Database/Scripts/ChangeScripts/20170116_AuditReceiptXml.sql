CREATE TABLE [dbo].[AuditReceiptXml]
(
	[RegistrationKpsID] [INT] NULL,
	[BookKpsID] [INT] NULL,
	[DeliveryDate] [BIGINT] NULL,
	[SecretaryKpsID] [INT] NULL,
	[LibraryKpsID] [INT] NULL,
	[AcademicKpsID] [INT] NULL,
	[BookCount] [INT] NULL,
	[Reason] [nvarchar](200) NULL,	
	[ReasonDate] [BIGINT] NULL,
	[Timestamp] [BIGINT] NULL,	
)
