CREATE PROCEDURE [dbo].[spExportCommitmentsRegistry] @phaseID int
AS
DECLARE @OtherPhaseID int
SELECT @OtherPhaseID = CASE WHEN @phaseID%2 = 0 THEN @phaseID - 1 ELSE @phaseID + 1 END
BEGIN
Select 
	i.InvoiceNumber,
	i.InvoiceDate,
	s.SupplierKpsID,
	s.Name,
	s.AFM,
	g.ID as GroupID,
	CASE WHEN g.State = 0 THEN 'Μη επιλεχθέν'
		WHEN g.State = 1 THEN 'Επιλεχθέν για πληρωμή'
		WHEN g.State = 2 THEN 'Εγκεκριμένη για πληρωμή'
		WHEN g.State = 3 THEN 'Σταλμένη προς ΥΔΕ'
		WHEN g.State = 4 THEN 'Επιστροφή από ΥΔΕ' END as GroupState,
	Round(i.InvoiceValue,2) as InvoiceValue,
	Round(gcatalogs.Amount, 2) as CatalogsAmount,
	p.OfficeSlipDate,
	g.PhaseID
from
	Invoice i
	left join CatalogGroup g on g.ID = i.GroupID
	left join (
		Select SupplierID,
               GroupID,
			   PhaseID,
               SUM(Amount) as Amount
		from catalog c
		where Status = 1
		and State in (0,1)
		and (@phaseID = 0 OR phaseID in (@phaseID, @OtherPhaseID))
		group by c.GroupID,SupplierID, PhaseID) as gcatalogs on gcatalogs.GroupID = g.ID
	left join paymentOrder p on p.groupID = g.ID 
		and p.state = 4
		and p.IsActive = 1
	left join supplier s on s.ID = g.SupplierID
where 
	i.IsActive = 1 
	and g.IsActive = 1
	and (@phaseID = 0 OR g.PhaseID in (@phaseID, @OtherPhaseID))
	and p.OfficeSlipDate is not null
order by i.InvoiceNumber
END

