CREATE PROCEDURE [dbo].[CardSearchWithoutSearchTerm] 
	@limitId bigint = 0,
	@categoryId bigint = 0,
	@subCategoryId bigint = 0,
	@attributeId bigint = 0,
	@typeId bigint = 0,
	@lvlRank int = 0,
	@atk int = 0,
	@def int = 0,
	@pageSize int = 10,
	@pageIndex int = 1,
	@filteredRowsCount INT OUTPUT
AS
BEGIN

	DECLARE @cardSearchResult TABLE
	(
		Id bigint,
		CardNumber bigint NULL,
		Name nvarchar(255) NOT NULL,
		[Description] nvarchar(max) NULL,
		CardLevel int NULL,
		CardRank int NULL,
		Atk int NULL,
		Def int NULL,
		CategoryId bigint NOT NULL,
		Category nvarchar(255) NOT NULL,
		SubCategories nvarchar(max),
		AttributeId bigint NULL,
		Attribute nvarchar(255) NULL,
		TypeId bigint NULL,
		[Type] nvarchar(255) NULL
	)

	-- Results before pagination
	INSERT INTO @cardSearchResult
	(
		Id,
		CardNumber,
		Name,
		[Description],
		CardLevel,
		CardRank,
		Atk,
		Def,
		CategoryId,
		Category,
		SubCategories,
		AttributeId,
		Attribute,
		TypeId,
		[Type]
	)
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
		(@limitId = 0 OR bc.LimitId = @limitId) AND
		(@categoryId = 0 OR c2.Id = @categoryId) AND
		(@subCategoryId = 0 OR sc.Id = @subCategoryId) AND	
		(@attributeId = 0 OR a.Id = @attributeId) AND
		(@lvlRank = 0 OR c.CardLevel = @lvlRank OR c.CardRank = @lvlRank) AND
		(@atk = 0 OR c.Atk = @atk) AND
		(@def = 0 OR c.Def = @def)

	-- Filtered row count before pagination is applied
	SELECT @filteredRowsCount = COUNT(Id) FROM @cardSearchResult csr

	SELECT
		Id,
		CardNumber,
		Name,
		Description,
		CardLevel,
		CardRank,
		Atk,
		Def,
		CategoryId,
		Category,
		SubCategories,
		AttributeId,
		Attribute,
		TypeId,
		Type
	FROM
		@cardSearchResult
	ORDER BY CardNumber DESC
	OFFSET @pageSize * (@pageIndex - 1) ROWS
	FETCH NEXT @pageSize ROWS ONLY
END