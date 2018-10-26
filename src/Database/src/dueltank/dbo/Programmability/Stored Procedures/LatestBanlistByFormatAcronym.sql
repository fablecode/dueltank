CREATE PROCEDURE [dbo].[LatestBanlistByFormatAcronym]
	@formatAcronym nvarchar(50),
	@releaseDate datetime2(3) OUTPUT
AS
BEGIN
	-- Banlist format
	DECLARE @formatId bigint = (SELECT f.Id	FROM dbo.Format f WHERE f.Acronym = @formatAcronym)

	-- Latest banlist id
	DECLARE @banlistId bigint = (SELECT TOP 1 b.Id FROM dbo.Banlist b WHERE b.FormatId = @formatId ORDER BY b.ReleaseDate DESC)

	-- Release date
	SELECT @releaseDate = b.ReleaseDate FROM dbo.Banlist b WHERE b.Id = @banlistId

	SELECT
		DISTINCT c.Id,
		l.Name AS Limit,
		c.CardNumber,
		c.Name,
		c.Description,
		c.CardLevel,
		c.CardRank,
		c.Atk,
		c.Def,
		c2.Id AS [CategoryId],
		c2.Name AS [Category],
		STUFF((SELECT ',' + 
					sc.Name
				FROM
					dbo.SubCategory sc
				INNER JOIN
					dbo.CardSubCategory cc ON cc.SubCategoryId = sc.Id
				WHERE
					cc.CardId = c.Id
				FOR XML PATH('')), 1, 1, '') AS SubCategories,
		a.Id AS [AttributeId],
		a.Name AS [Attribute],
		t.Id AS [TypeId],
		t.Name AS [Type]
	FROM
		dbo.Card c
	INNER JOIN
		dbo.CardSubCategory csc ON c.Id = csc.CardId
	INNER JOIN
		dbo.SubCategory sc ON csc.SubCategoryId = sc.Id
	INNER JOIN
		dbo.Category c2 ON sc.CategoryId = c2.Id
	INNER JOIN
		dbo.BanlistCard bc ON bc.CardId = c.Id
	INNER JOIN
		dbo.Banlist b ON b.Id = bc.BanlistId
	LEFT JOIN
		dbo.Limit l ON l.Id = bc.LimitId
	LEFT JOIN
		dbo.CardAttribute ca ON c.Id = ca.CardId
	LEFT JOIN
		dbo.Attribute a ON ca.AttributeId = a.Id
	LEFT JOIN
		dbo.CardType ct ON c.Id = ct.CardId
	LEFT JOIN
		dbo.[Type] t ON ct.TypeId = t.Id
	WHERE
		(bc.BanlistId = @banlistId)
END