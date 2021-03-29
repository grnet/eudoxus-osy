CREATE TABLE report.ViewStatisticsPerDepartment (
	[DepartmentID] [int] NOT NULL,
	[Debt] [decimal](10, 2) NULL,
	[PhaseID] [int] NOT NULL,
	[InstName] [nvarchar](256) NULL,
	[DepName] [nvarchar](256) NULL,
	[PricedCount] [int] NULL,
	[NotPricedCount] [int] NULL
	) 