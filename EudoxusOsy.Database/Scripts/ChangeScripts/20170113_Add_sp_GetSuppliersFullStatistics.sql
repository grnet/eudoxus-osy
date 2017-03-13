CREATE PROCEDURE sp_GetSuppllierFullStatistics @supplier_kpsid int, @afm nvarchar(10), @name nvarchar(50), 
		@supplier_type int, @Count int OUTPUT
AS
BEGIN
SELECT * FROM [report].SuppliersFullStatistics
where 
	(@supplier_kpsid is null OR @supplier_kpsid = supplier_kpsid)
	AND (@afm is null OR @afm = taxRoll_number)
	AND (@name is null or official_name like '%' + @name + '%')
	AND (@supplier_type is null OR @supplier_type = supplierType)
SET @Count = @@ROWCOUNT
END

