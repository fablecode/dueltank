CREATE TABLE [dbo].[ArchetypeCard] (
    [ArchetypeId] BIGINT NOT NULL,
    [CardId]      BIGINT NOT NULL,
    CONSTRAINT [PK_ArchetypeCard] PRIMARY KEY CLUSTERED ([ArchetypeId] ASC, [CardId] ASC)
);

