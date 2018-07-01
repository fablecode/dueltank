CREATE TABLE [dbo].[DeckCard] (
    [DeckTypeId] BIGINT NOT NULL,
    [DeckId]     BIGINT NOT NULL,
    [CardId]     BIGINT NOT NULL,
    [Quantity]   INT    NOT NULL,
    [SortOrder]  INT    NOT NULL,
    CONSTRAINT [PK_DeckCard] PRIMARY KEY CLUSTERED ([DeckTypeId] ASC, [DeckId] ASC, [CardId] ASC),
    CONSTRAINT [FK_DeckCard_Card] FOREIGN KEY ([CardId]) REFERENCES [dbo].[Card] ([Id]),
    CONSTRAINT [FK_DeckCard_Deck] FOREIGN KEY ([DeckId]) REFERENCES [dbo].[Deck] ([Id]),
    CONSTRAINT [FK_DeckCard_DeckType1] FOREIGN KEY ([DeckTypeId]) REFERENCES [dbo].[DeckType] ([Id])
);

