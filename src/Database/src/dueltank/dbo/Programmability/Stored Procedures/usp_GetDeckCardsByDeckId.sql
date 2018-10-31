-- =============================================
-- Author:		fablecode
-- Create date: 29/06/2018
-- Description:	Retrieve deck cards by deck id
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetDeckCardsByDeckId]
	@DeckId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	SELECT
		DISTINCT c.Id,c.Id AS DeckDetailId, c.Id AS DeckDetailId1, c.Id AS DeckDetailId2,
		c.CardNumber,
		c.Name,
		c.Description,
		c.CardLevel,
		c.CardRank,
		c.Atk,
		c.Def,
		c.Created,
		c.Updated,
		dt.Name AS [DeckType],
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
		a.Name AS [Attribute],
		a.Id AS [AttributeId],
		t.Id AS [TypeId],
		t.Name AS [Type],
		dc.Quantity,
		dc.SortOrder	
	FROM
		dbo.DeckCard dc
	INNER JOIN
		dbo.DeckType dt ON dc.DeckTypeId = dt.Id
	INNER JOIN	
		dbo.Deck d ON dc.DeckId = d.Id
	INNER JOIN
		dbo.Card c ON dc.CardId = c.Id
	LEFT JOIN
		dbo.CardSubCategory csc ON c.Id = csc.CardId
	LEFT JOIN
		dbo.SubCategory sc ON csc.SubCategoryId = sc.Id
	LEFT JOIN
		dbo.Category c2 ON sc.CategoryId = c2.Id
	LEFT JOIN
		dbo.CardAttribute ca ON c.Id = ca.CardId
	LEFT JOIN
		dbo.Attribute a ON ca.AttributeId = a.Id
	LEFT JOIN
		dbo.CardType ct ON c.Id = ct.CardId
	LEFT JOIN
		dbo.[Type] t ON ct.TypeId = t.Id
	WHERE d.Id = @DeckId
	ORDER BY SortOrder ASC
END