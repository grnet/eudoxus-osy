CREATE TABLE [dbo].[KpsOnly](
 [KpsRegistrationID] [bigint] NOT NULL,
 [BookKpsID] [bigint] NOT NULL,
 [DepartmentKpsID] [bigint] NOT NULL,
 [Amount] [int] NOT NULL
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170119-105256] ON [dbo].[KpsOnly]
(
 [KpsRegistrationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ReceiptsOnly](
 [KpsRegistrationID] [bigint] NOT NULL,
 [BookKpsID] [bigint] NOT NULL,
 [DeparmtentKpsID] [bigint] NOT NULL,
 [Amount] [int] NOT NULL
) ON [PRIMARY]

GO


CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170119-105257] ON [dbo].[ReceiptsOnly]
(
 [KpsRegistrationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO