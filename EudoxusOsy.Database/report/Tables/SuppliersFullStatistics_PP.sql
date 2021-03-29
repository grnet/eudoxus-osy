CREATE TABLE [dbo].[SuppliersFullStatistics_PP]
(
	[supplier_id] [int] NULL,
	[supplier_kpsid] [int] NULL,
	[publicFinancialOffice] [nvarchar](500) NULL,
	[paymentPfo] [nvarchar](50) NULL,
	[contact_name] [nvarchar](200) NULL,
	[taxRoll_number] [nvarchar](20) NULL,
	[official_name] [nvarchar](200) NULL,
	[supplierType] [int] NULL,
	[totalprice] [decimal](10,2) NULL,
	[totalpayment] [decimal](10,2) NULL,
	[totalofferprice] [decimal](10,2) NULL,
	[totaltoyde] [decimal](10,2) NULL,
	[status] [int] NULL,
	[phase_id] [int] NULL,
	[booksPriced] [int] NULL,
	[booksNotPriced] [int] NULL
)
