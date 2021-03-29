CREATE TABLE report.ViewStatisticsPerInstitution (
		[InstitutionID] INT NOT NULL,	
		[Debt] [decimal](10, 2) NULL,
		[PhaseId] [int] NULL,
		[InstName] [nvarchar](500) NULL,			
		[PricedCount] [int] NULL,
		[NotPricedCount] [int] NULL,
		[DepartmentCount] [int] NULL,
	) 