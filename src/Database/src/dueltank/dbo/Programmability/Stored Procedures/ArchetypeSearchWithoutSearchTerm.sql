CREATE PROCEDURE [dbo].[ArchetypeSearchWithoutSearchTerm]
	@PageSize int = 8,
	@PageIndex int = 1,
	@TotalRowsCount int OUTPUT
AS
	DECLARE @ArchetypeList TABLE
	(
		 [Id] bigint
		,[Name] NVARCHAR(255)
		,[Url] NVARCHAR(2083)
		,[Created] datetime
		,[Updated] datetime
	)

	INSERT INTO @ArchetypeList
	(
		Id,
		Name,
		Url,
		Created,
		Updated
	)
	SELECT
		DISTINCT a.Id,
		a.Name,
		a.Url,
		a.Created,
		a.Updated
	FROM
		dbo.Archetype a
	INNER JOIN
		ArchetypeCard ac ON (a.Id = ac.ArchetypeId)

	-- Filtered row count before pagination is applied
	SELECT @TotalRowsCount = COUNT(Id) FROM @ArchetypeList

	SELECT
		*
	FROM
		@ArchetypeList
	ORDER BY [@ArchetypeList].Id
	OFFSET @PageSize * (@PageIndex - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY