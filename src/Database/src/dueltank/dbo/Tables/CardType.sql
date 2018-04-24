CREATE TABLE [dbo].[CardType] (
    [TypeId] BIGINT NOT NULL,
    [CardId] BIGINT NOT NULL,
    CONSTRAINT [PK_CardType] PRIMARY KEY CLUSTERED ([TypeId] ASC, [CardId] ASC)
);

