CREATE VIEW [dbo].[BookPricesGridV]
	AS 

	SELECT b.ID AS BookID, b.BookKpsID, b.Title, b.Author, b.Publisher, b.ISBN, b.IsActive,
	bp.CreatedAt, bp.SuggestedPrice, bp.Price, 
	ISNULL(bp.PendingCommitteePriceVerification ,0) AS HasPendingPriceVerification, 
	ISNULL(CAST(CASE WHEN bp.HasUnexpectedPriceChangePhaseID > 0 THEN 1 ELSE 0 END AS bit),0) AS HasUnexpectedPriceChange,
	bp.ChangeYear,
	bp.PriceChecked,
	bp.ID AS BookPriceID,
	ROW_NUMBER() OVER(ORDER BY b.ID) AS ID
  FROM Book b 
  LEFT JOIN BookPricesV bp ON bp.BookID = b.ID AND bp.Approved IS NULL
  WHERE (b.PendingCommitteePriceVerification = 1 OR b.HasUnexpectedPriceChangePhaseID IS NOT NULL)