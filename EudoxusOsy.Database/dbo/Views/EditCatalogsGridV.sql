
CREATE VIEW [dbo].[EditCatalogsGridV]
	AS
	

	SELECT
	c.ID, 
	c.GroupID,
	c.PhaseID,
	s.SupplierKpsID,
	c.Percentage,
	d.LibraryKpsID,
	d.SecretaryKpsID,
	ISNULL(d.LibraryKpsID,d.SecretaryKpsID) AS SecretaryOrLibaryID,
	b.BookKpsID,
	c.BookCount,
	discount.DiscountPercentage,
	c.Amount,
	cg.State AS GroupState,
	c.CreatedAt,
	c.Status,
	c.State,
	c.CatalogType
	FROM Catalog c
	JOIN Phase p ON p.ID = c.PhaseID
	JOIN Book b ON b.ID = c.BookID
	JOIN Supplier s ON s.ID = c.SupplierID
	JOIN Department d ON d.ID = c.DepartmentID
	JOIN Discount discount ON discount.ID = c.DiscountID
	LEFT JOIN CatalogGroup cg ON cg.ID = c.GroupID	
	WHERE p.IsActive = 1



GO

		 
		   