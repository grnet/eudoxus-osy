  Update dbo.[File]
  SET PdfNotes = pf.F2
  from dbo.[File] f
  inner join parsedfiles pf on  pf.F1 = RIGHT(f.PathName, CHARINDEX('/', REVERSE(f.pathname)) -1)

ALTER TABLE CatalogGroupLog 
ADD Comments nvarchar(max) null, 
Amount float null,
PdfNotes nvarchar(500) null

GO

ALTER TABLE CatalogGroupLog 
ADD OfficeSlipNumber int null,
OfficeSlipDate datetime2(7) null 
GO



Insert Into CatalogGroupLog (GroupID, OldState, NewState, OfficeSlipNumber, OfficeSlipDate, Amount, OldValues,NewValues,Comments, PdfNotes, CreatedBy, CreatedAt)
Select GroupID, null, State, OfficeSlipNumber, OfficeSlipDate, Amount, null, null, Comments, f.PdfNotes, po.CreatedBy, po.CreatedAt
from PaymentOrder po
left join dbo.[File] f on po.FileID = f.ID 


