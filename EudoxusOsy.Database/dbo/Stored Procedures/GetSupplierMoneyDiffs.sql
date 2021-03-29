CREATE PROCEDURE dbo.GetSupplierMoneyDiffs @PhaseID int
AS
BEGIN 
IF OBJECT_ID('dbo.TempAmountDiffs', 'U') IS NOT NULL 
  DROP TABLE dbo.TempAmountDiffs; 

SELECT a.SupplierID, Convert(decimal(12,4),AMNT1) as SupplierPhaseAmount,  Convert(decimal(12,4),AMNT) as CatalogsSumAmount, Convert(decimal(12,4),AMNT1) - Convert(decimal(12,4),AMNT) as AmountDiff
INTO dbo.TempAmountDiffs
FROM (
SELECT SupplierID, SUM(TotalDebt) AS AMNT1
FROM SupplierPhase 
WHERE PhaseID = @PhaseID
GROUP BY SupplierID
) a
LEFT JOIN (
SELECT SUpplierID, SUM(Amount) AS AMNT
FROM Catalog
WHERE PhaseID = @PhaseID
GROUP BY SUpplierID
) b ON a.SupplierID = b.SupplierID
WHERE a.AMNT1 > b.AMNT
END