ALTER PROCEDURE [dbo].[spCatalogsReport]
	@phaseId int = 0
AS
BEGIN
	
	SELECT b.Title AS BookTitle, b.BookKpsID, s.Name AS SupplierName, s.SupplierKpsID, 
		d.Name AS Department, d.LibraryKpsID, d.SecretaryKpsID, c.Amount AS FinalAmount, 
		c.BookCount, bp.Price AS PricePerBook, b.FirstRegistrationYear, Discount.DiscountPercentage, bs.Percentage, c.HasPendingPriceVerification, 
		Discount.BookCountFrom, Discount.BookCountTo
	FROM Catalog c
	JOIN Phase p ON p.ID = c.PhaseID
	JOIN Book b ON b.ID = c.BookID
	JOIN Department d ON d.ID = c.DepartmentID
	LEFT JOIN BookPrice bp ON bp.ID = c.BookPriceID AND bp.Status = 1
	JOIN Supplier s ON s.ID = c.SupplierID
	JOIN Discount ON Discount.ID = c.DiscountID
	JOIN BookSupplier bs ON bs.SupplierID = s.ID AND bs.BookID = b.ID AND bs.Year = p.Year AND bs.IsActive = 1
	WHERE c.PhaseID = @phaseId
	
END
