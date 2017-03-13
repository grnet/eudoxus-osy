
ALTER PROCEDURE [dbo].[spSuppliersNoLogisticBooks] @year int
AS
BEGIN

Select s.SupplierKpsID,
	s.Name AS SupplierName,
	sd.PublisherAddress,
	sd.PublisherZipCode,
	cit.Name as CityName,
	s.AFM,
	s.pfoID as PfoID,
	case when PfoID = -1 THEN s.Pfo else pfo.Name END as PFOffice,
	Round(sum(c.amount),2) as TotalAmount
from 
	Supplier s
left join Reporter r on s.ReporterID = r.ID
left join SupplierDetails sd on sd.ID = s.SupplierKpsID
left join Catalog c on c.SupplierID = s.ID
left join CatalogGroup cg on cg.ID = c.GroupID
left join PaymentOrder p on p.GroupID = cg.ID
left join Kap_Cities cit on cit.ID = sd.PublisherCityID
left join PublicFinancialOffice pfo on pfo.ID = s.PfoID
where s.Status = 1
	and HasLogisticBooks = 0
	and sd.PublisherZipCode <> '00000'
	and c.Status = 1
	and cg.IsActive = 1
	and cg.State = 3
	and p.IsActive = 1
	and p.State = 4 
	and p.OfficeSlipDate is not null
	and (@year = 0 OR 
	(p.OfficeSlipDate > cast(cast(CASE WHEN @year = 0 then 2000 else @year end as nvarchar(4)) + '-1-1' as datetime)
	and p.OfficeSlipDate < cast(cast(CASE WHEN @year = 0 then 2000 else @year end as nvarchar(4)) + '-12-31' as datetime)))
	group by s.SupplierKpsID, S.Name, SD.PublisherAddress, SD.PublisherZipCode, cit.Name, S.AFM, s.PfoID, s.Pfo, pfo.Name
	order by totalamount desc
END
