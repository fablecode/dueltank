CREATE PROCEDURE [dbo].[DeckSearchWithoutSearchTerm]
	@PageSize int = 8,
	@PageIndex int = 1,
	@TotalRowsCount int OUTPUT
AS
	DECLARE @DeckList TABLE
	(
		[Id] bigint
		,[UserId] NVARCHAR(128)
		,[UserName] NVARCHAR(256)
		,[Name] NVARCHAR(255)
		,[Description] NVARCHAR(max)
		,[Created] datetime
		,[Updated] datetime
		,[TotalCards] int
	)


	INSERT INTO @DeckList
	(
		Id,
		UserId,
		UserName,
		Name,
		Description,
		Created,
		Updated,
		TotalCards
	)
	SELECT
		d.Id,
		d.UserId,
		anu.UserName,
		d.Name,
		d.Description,
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
		d.Created,
		d.Updated

	SELECT @TotalRowsCount = COUNT(*) FROM @DeckList

	SELECT
		*
	FROM
		@DeckList
	ORDER BY Updated DESC
	OFFSET @pageSize * (@pageIndex - 1) ROWS
	FETCH NEXT @pageSize ROWS ONLY