CREATE FUNCTION [dbo].[fnCalculateCatalogAmount]
(
	@BookCount INT,
	@BookUnitPrice decimal (5,2),
	@Percentage decimal (5,2),
	@BookDiscountPercentage decimal (5,2),
	@YearDiscountPercentage decimal (5,2),
	@PerYearDiscountPercentage decimal (5,2)
)
RETURNS decimal (8,2)
AS
BEGIN
		
	RETURN @BookCount * 
			ROUND(@BookUnitPrice * 
					(@Percentage/100) *
					IIF(@BookDiscountPercentage > 0, @BookDiscountPercentage, @YearDiscountPercentage) * @PerYearDiscountPercentage 
				,2)

END

GO