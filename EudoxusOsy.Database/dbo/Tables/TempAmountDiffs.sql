CREATE TABLE [dbo].[TempAmountDiffs]
(
	[SupplierID] INT NOT NULL PRIMARY KEY, 
    [SupplierPhaseAmount] DECIMAL(8, 2) NOT NULL, 
    [CatalogsSumAmount] DECIMAL(8, 2) NOT NULL, 
    [AmountDiff] DECIMAL(8, 2) NULL
)
