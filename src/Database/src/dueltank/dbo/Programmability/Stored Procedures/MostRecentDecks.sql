CREATE PROCEDURE [dbo].[MostRecentDecks]
	@PageSize int = 8
AS
BEGIN
	SELECT
		TOP (@PageSize)
		d.Id,
		d.UserId,
		anu.UserName,
		d.Name,
		d.Description,
		d.VideoUrl,
		d.Created,
		d.Updated,
		SUM(dc.Quantity) AS TotalCards
	FROM
		dbo.Deck d
	INNER JOIN
		dbo.AspNetUsers anu ON (anu.Id = d.UserId)
	INNER JOIN
		dbo.DeckCard dc ON (dc.DeckId = d.Id)
	INNER JOIN
		dbo.DeckType dt ON (dc.DeckTypeId = dt.Id)
	WHERE
		dt.Name = 'Main'
	GROUP BY
		d.Id, 
		d.UserId, 
		anu.UserName, 
		d.Name, 
		d.Description, 
		d.VideoUrl,
		d.Created,
		d.Updated
	ORDER BY
		d.Created DESC
END