CREATE PROCEDURE [dbo].[MostRecentArchetypes]
	@PageSize int = 10
AS
BEGIN
	SELECT
		TOP (@PageSize)
		a.Id,
		a.Name,
		a.Url,
		a.Created,
		a.Updated,
		COUNT(ac.CardId) AS TotalCards
	FROM
		dbo.Archetype a
	INNER JOIN
		ArchetypeCard ac ON (a.Id = ac.ArchetypeId)
	GROUP BY
		a.Id, a.Name, a.Url, a.Created, a.Updated
	ORDER BY
		a.Created DESC
END