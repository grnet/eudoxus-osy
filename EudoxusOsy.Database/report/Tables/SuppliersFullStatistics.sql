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