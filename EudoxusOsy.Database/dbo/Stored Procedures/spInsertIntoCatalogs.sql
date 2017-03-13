CREATE PROCEDURE [dbo].[spInsertIntoCatalogs]
        @PhaseID int, 
		@OriginalPhaseID int, 
		@BookID int, 
		@SupplierID int, 
		@DepartmentID int, 
		@DiscountID int, 
		@BookCount int, 
		@Percentage decimal(5,2), 
		@BookPriceID int, 
		@Amount decimal(8,2)
AS 
Begin
	Insert Into Catalog (CatalogType, PhaseID, OriginalPhaseID, BookID, SupplierID, DepartmentID, DiscountID, 
		BookCount, Percentage, BookPriceID, Amount, 
		State, Status, CreatedAt, CreatedBy)
	Values (0, @PhaseID, @OriginalPhaseID, @BookID, @SupplierId, @DepartmentID, @DiscountID, 
		@BookCount, @Percentage, @BookPriceID, @Amount, 
		0, 1, GETDATE(), 'sysadmin')
END

