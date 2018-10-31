CREATE PROCEDURE [dbo].[MostRecentBanlists]
AS
BEGIN
	DECLARE @LatestBanlist TABLE
	(
		 Id int NOT NULL identity(1,1),
		 [Acronym] NVARCHAR(128)
		,[Name] NVARCHAR(256)
		,[ReleaseDate] datetime
	)

	-- Banlist format
	DECLARE @tcgId bigint = (SELECT f.Id	FROM dbo.[Format] f WHERE f.Acronym = 'TCG')
	DECLARE @ocgId bigint =  (SELECT f.Id	FROM dbo.[Format] f WHERE f.Acronym = 'OCG')

	-- Latest TCG banlist
	INSERT INTO @LatestBanlist
	(
	    Acronym,
	    Name,
	    ReleaseDate
	)
	SELECT TOP (1) f.Acronym, bl.Name, ReleaseDate FROM Banlist bl	INNER JOIN Format f ON (bl.FormatId = f.Id) WHERE f.Id = @tcgId ORDER BY bl.ReleaseDate DESC

	-- Latest OCG banlist
	INSERT INTO @LatestBanlist
	(
	    Acronym,
	    Name,
	    ReleaseDate
	)
	SELECT TOP (1) f.Acronym, bl.Name, ReleaseDate FROM Banlist bl	INNER JOIN Format f ON (bl.FormatId = f.Id) WHERE f.Id = @ocgId ORDER BY bl.ReleaseDate DESC

	SELECT 
		Acronym,
	    Name,
	    ReleaseDate 
	FROM 
		@LatestBanlist lb ORDER BY lb.Id ASC

END