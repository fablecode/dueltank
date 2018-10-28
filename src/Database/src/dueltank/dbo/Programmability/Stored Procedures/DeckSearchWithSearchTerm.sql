CREATE PROCEDURE [dbo].[DeckSearchWithSearchTerm]
	@SearchText nvarchar(500),
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
		,[Rank] int
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
		TotalCards,
		[Rank]
	)
	SELECT
		d.Id,
		d.UserId,
		anu.UserName,
		d.Name,
		d.Description,
		d.Created,
		d.Updated,
		SUM(dc.Quantity) AS TotalCards,
		KEY_TBL.Rank
	FROM
		dbo.Deck d
	INNER JOIN
		dbo.AspNetUsers anu ON (anu.Id = d.UserId)
	INNER JOIN
		dbo.DeckCard dc ON (dc.DeckId = d.Id)
	INNER JOIN
		dbo.DeckType dt ON (dc.DeckTypeId = dt.Id)
	INNER JOIN
		CONTAINSTABLE ([dbo].[Deck], ([Name], [Description]), @SearchText) AS KEY_TBL 
			ON ([d].[Id] = KEY_TBL.[KEY])
	WHERE
		dt.Name = 'Main'
	GROUP BY
		d.Id, 
		d.UserId, 
		anu.UserName, 
		d.Name, 
		d.Description, 
		d.Created,
		d.Updated,
		KEY_TBL.Rank

	SELECT @TotalRowsCount = COUNT(*) FROM @DeckList

	SELECT
		Id,
		UserId,
		UserName,
		Name,
		Description,
		Created,
		Updated,
		TotalCards
	FROM
		@DeckList
	ORDER BY [Rank]
	OFFSET @pageSize * (@pageIndex - 1) ROWS
	FETCH NEXT @pageSize ROWS ONLY