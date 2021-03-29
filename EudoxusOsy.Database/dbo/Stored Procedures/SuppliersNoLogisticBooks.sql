CREATE PROCEDURE [dbo].[spSuppliersNoLogisticBooks] @year int
AS
BEGIN

Select s.SupplierKpsID,
	s.Name AS SupplierName,
	sd.PublisherAddress,
	sd.PublisherZipCode,
	cit.Name as CityName,
	s.AFM,
	s.DOY as DOY,	
	Round(sum(c.amount),2) as TotalAmount
from 
	Supplier s
left join Reporter r on s.ReporterID = r.ID
left join SupplierDetails sd on sd.ID = s.SupplierKpsID
left join Catalog c on c.SupplierID = s.ID
left join Phase p on p.ID = c.PhaseID
left join Kap_Cities cit on cit.ID = sd.PublisherCityID
where s.Status = 1
	and HasLogisticBooks = 0
	and sd.PublisherZipCode <> '00000'
	and c.Status = 1
	and (ISNULL(@year,0) = 0 OR p.Year = @year)
	group by s.SupplierKpsID, S.Name, SD.PublisherAddress, SD.PublisherZipCode, cit.Name, S.AFM, s.DOY
	order by totalamount desc
END