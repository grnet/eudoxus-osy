CREATE SCHEMA [report]
GO

--GIVE PERMISSIONS TO THE NEW SCHEMA!!!!!!!!!!!!!!!!!!!!!!!!!!!!
------------------------------



IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='ViewTotalSum' and xtype='U')
    CREATE TABLE report.ViewTotalSum (
		phase_id INT NOT NULL,
		totalSum FLOAT NOT NULL		
	) 
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='ViewStatisticsPerDepartment' and xtype='U')
    CREATE TABLE report.ViewStatisticsPerDepartment (
		department_id INT NOT NULL,
		debt FLOAT NOT NULL,
		phase_id INT NOT NULL
	) 
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='ViewStatisticsPerInstitution' and xtype='U')
    CREATE TABLE report.ViewStatisticsPerInstitution (
		institution_id INT NOT NULL,
		debt FLOAT NOT NULL,
		phase_id INT NOT NULL
	) 
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='StatsNotPricedPerDepartment' and xtype='U')
    CREATE TABLE report.StatsNotPricedPerDepartment (		
		book_id INT NOT NULL,
		times_registered INT NULL,
		book_title NVARCHAR(500), 
		book_kpsid INT, 
		department_id INT, 
		phase_id INT NOT NULL
	) 
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='StatsPricedPerDepartment' and xtype='U')
    CREATE TABLE report.StatsPricedPerDepartment (		
		book_id INT NOT NULL,
		times_registered INT NULL,
		book_title NVARCHAR(500), 
		book_kpsid INT, 
		department_id INT, 
		phase_id INT NOT NULL
	) 
GO
 
 IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='StatsPricedPerDepartment' and xtype='U')
    CREATE TABLE report.StatsPricedPerDepartment (		
		book_id INT NOT NULL,
		times_registered INT NULL,
		book_title NVARCHAR(500), 
		book_kpsid INT, 
		department_id INT, 
		phase_id INT NOT NULL
	) 
GO

 IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='SuppliersFullStatistics' and xtype='U')
	CREATE TABLE report.SuppliersFullStatistics (
		supplier_id INT,
		supplier_kpsid INT,
		publicFinancialOffice NVARCHAR(200),
		paymentPfo NVARCHAR(50),
		contact_name NVARCHAR(200),
		taxRoll_number NVARCHAR(20),
		official_name NVARCHAR(200),
		supplierType INT,
		totalprice float,
		totalpayment float,
		totalofferprice float,
		totaltoyde float,
		phase_id INT
	)
 GO


          