
CREATE PROCEDURE [dbo].[sp_GetSuppllierFullStatistics] @supplier_kpsid int, @afm nvarchar(10), @PhaseID int, @name nvarchar(50), 
		@supplier_type int, @Count int OUTPUT
AS
BEGIN

	DECLARE @currentPhaseID int
	SELECT @currentPhaseID = ID from Phase p where p.IsActive = 1 and EndDate is null

	IF(@phaseID = @currentPhaseID)
	BEGIN
		SELECT * FROM [report].SuppliersFullStatistics
		where 
			(@supplier_kpsid is null OR @supplier_kpsid = supplier_kpsid)
			AND (@afm is null OR @afm = taxRoll_number)
			AND (@name is null or official_name like '%' + @name + '%')
			AND (@supplier_type is null OR @supplier_type = supplierType)
			AND phase_id = @PhaseID
		SET @Count = @@ROWCOUNT
	END
	ELSE
	BEGIN 
		SELECT * FROM [report].SuppliersFullStatistics_PP
		where 
			(@supplier_kpsid is null OR @supplier_kpsid = supplier_kpsid)
			AND (@afm is null OR @afm = taxRoll_number)
			AND (@name is null or official_name like '%' + @name + '%')
			AND (@supplier_type is null OR @supplier_type = supplierType)
			AND phase_id = @PhaseID
		SET @Count = @@ROWCOUNT
	END
END
