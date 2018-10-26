CREATE PROCEDURE [dbo].[CardSearchByName]
	@name nvarchar(255)
AS
BEGIN
	SELECT
		DISTINCT c.Id,
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
	LEFT JOIN
		dbo.BanlistCard bc ON bc.CardId = c.Id
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
		(c.Name = @name)
END