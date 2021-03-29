CREATE TABLE [dbo].[BusinessLogicErrorsKPS]
(
	[ID] INT NOT NULL PRIMARY KEY IDENTITY,
	[EntityID] int not null,
	[ErrorEntityType] int not null,
	[ErrorCode] int not null,
	[Description] nvarchar(500) not null,
	[CreatedAt] datetime2(7) not null
)
