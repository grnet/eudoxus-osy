ALTER FUNCTION [dbo].[fnCalculateCatalogAmount]
(
	@BookCount INT,
	@BookUnitPrice decimal (5,2),
	@Percentage decimal (5,2),
	@BookDiscountPercentage decimal (5,2),	
	@PerYearDiscountPercentage decimal (5,2)
)
RETURNS decimal (8,2)
AS
BEGIN
		
	RETURN @BookCount * 
			ROUND(@BookUnitPrice * 
					(@Percentage/100) *
					(@BookDiscountPercentage - @PerYearDiscountPercentage)
				,2)

END
GO