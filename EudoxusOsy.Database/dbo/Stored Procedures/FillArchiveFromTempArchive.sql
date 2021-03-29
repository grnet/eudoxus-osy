CREATE PROCEDURE [dbo].[FillArchiveFromTempArchive]
AS
BEGIN
	
	Update Archive SET IsActive = 0
	WHERE IsActive = 1

	Update Archive 
	SET KPSArchiveID = t.KPSArchiveID,
		Price = t.PRice,
		SuggestedPrice = t.SuggestedPrice,
		Fek = t.Fek,
		LastUpdate = t.LastUpdate, 
		DecisionNumber = t.DecisionNumber,
		PriceComments = t.PriceComments,
		CheckedPrice = t.CheckedPrice,
		IsActive = 1
	from TempArchive t 
	inner join Archive a on a.BookKpsID = t.BookId and a.Year = t.Year
	
	INSERT INTO Archive (BookKpsID, KPSArchiveID, Year, Price, SuggestedPrice, Fek, LastUpdate, DecisionNumber, PriceComments, CheckedPrice, IsActive)
	Select t.BookID, t.KPSArchiveID, t.Year, t.Price, t.SuggestedPrice, t.Fek, t.LastUpdate, t.DecisionNumber, t.PriceComments, t.CheckedPrice, 1 
	from TempArchive t 
	left join Archive a on a.BookKpsID = t.BookID and a.Year = t.Year
	where a.ID is null
		
	Delete from TempArchive
	
	Return 1
END

