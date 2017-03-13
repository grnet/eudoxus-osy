ALTER TABLE SupplierDetails
ADD SelfPublisherIdentificationIssueDateDate datetime2(7),
LegalPersonIdentificationIssueDateDate datetime2(7),
ContactIdentificationIssueDateDate datetime2(7)

GO
--Να μην τρέξει με την μία το Script!!!!!
--Msg 242, Level 16, State 3, Line 2
--The conversion of a nvarchar data type to a datetime data type resulted in an out-of-range value.

Update SupplierDetails
SET SelfPublisherIdentificationIssueDateDate = CONVERT(datetime, Cast(SelfPublisherIdentificationIssueDate as nvarchar(10)), 102),
LegalPersonIdentificationIssueDateDate = CONVERT(datetime,Cast(LegalPersonIdentificationIssueDate as nvarchar(10)), 102),
ContactIdentificationIssueDateDate =  CONVERT(datetime, Cast(ContactIdentificationIssueDate as nvarchar(10)), 102)


Update SupplierDetails
SET LegalPersonIdentificationIssueDate = NULL,
SelfPublisherIdentificationIssueDate = NULL,
ContactIdentificationIssueDate = NULL



ALTER TABLE SupplierDetails ALTER COLUMN LegalPersonIdentificationIssueDate DATETIME2(7) NULL
ALTER TABLE SupplierDetails ALTER COLUMN SelfPublisherIdentificationIssueDate DATETIME2(7) NULL
ALTER TABLE SupplierDetails ALTER COLUMN ContactIdentificationIssueDate DATETIME2(7) NULL

Update SupplierDetails
SET SelfPublisherIdentificationIssueDate = SelfPublisherIdentificationIssueDateDate,
LegalPersonIdentificationIssueDate = LegalPersonIdentificationIssueDateDate,
ContactIdentificationIssueDate = ContactIdentificationIssueDateDate

ALTER TABLE SupplierDetails
DROP Column SelfPublisherIdentificationIssuedateDate,
LegalPersonIdentificationIssueDateDate,
ContactIdentificationIssueDateDate