CREATE TABLE [dbo].[Archetype] (
    [Id]      BIGINT         NOT NULL,
    [Name]    NVARCHAR (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Url]     VARCHAR (2083) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created] DATETIME2 (7)  NOT NULL,
    [Updated] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Archetypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE FULLTEXT INDEX ON [dbo].[Archetype]
    ([Name] LANGUAGE 1033)
    KEY INDEX [PK_Archetypes]
    ON [ArchetypeCatalog];

