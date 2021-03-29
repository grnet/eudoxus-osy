CREATE TABLE report.SuppliersFullStatistics (
		supplier_id INT,
		supplier_kpsid INT,
		publicFinancialOffice NVARCHAR(200),
		paymentPfo NVARCHAR(50),
		contact_name NVARCHAR(200),
		taxRoll_number NVARCHAR(20),
		official_name NVARCHAR(200),
		supplierType INT,
		totalprice decimal(10,2) NULL,
		totalpayment decimal(10,2) NULL,
		totalofferprice decimal(10,2) NULL,
		totaltoyde decimal(10,2) NULL,
		phase_id INT,
		booksPriced INT,
		booksNotPriced INT
	)