ALTER TABLE PaymentOrder ADD OfficeSlipDateTemp DATETIME2(7) NULL

GO

UPDATE PaymentOrder
SET OfficeSlipDateTemp = DATEADD(dd, 0, DATEDIFF(dd, 0, DATEADD(SECOND, CAST(OfficeSlipDate as int), '19700101')))

GO

ALTER TABLE PaymentOrder DROP COLUMN OfficeSlipDate

GO

ALTER TABLE PaymentOrder ADD OfficeSlipDate DATETIME2(7) NULL

GO

UPDATE PaymentOrder
SET OfficeSlipDate = OfficeSlipDateTemp

GO

ALTER TABLE PaymentOrder DROP COLUMN OfficeSlipDateTemp
