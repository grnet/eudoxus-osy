CREATE VIEW [dbo].[BookPricesV]
	AS
	
	SELECT *
	FROM (
		SELECT bpc.ID, b.ID AS BookID, b.BookKpsID, b.Title, 
		bpc.ID AS BookPriceChangeID, bpc.CreatedAt, bpc.SuggestedPrice, bpc.Price, bpc.PriceChecked,
		b.PendingCommitteePriceVerification, b.HasUnexpectedPriceChangePhaseID, bpc.Year AS ChangeYear, bpc.Approved,
		ROW_NUMBER() OVER(PARTITION BY b.ID, bpc.YEAR ORDER BY bpc.CreatedAt DESC ) AS RowNum
		FROM [Book] b
		JOIN [BookPriceChange] bpc ON b.ID = bpc.BookID
		) a
	WHERE a.RowNum = 1



	