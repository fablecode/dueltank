CREATE TABLE [dbo].[BanlistCard] (
    [BanlistId] BIGINT NOT NULL,
    [CardId]    BIGINT NOT NULL,
    [LimitId]   BIGINT NOT NULL,
    CONSTRAINT [PK_BanlistCard] PRIMARY KEY CLUSTERED ([BanlistId] ASC, [CardId] ASC)
);

