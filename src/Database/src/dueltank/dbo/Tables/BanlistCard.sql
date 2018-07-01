CREATE TABLE [dbo].[BanlistCard] (
    [BanlistId] BIGINT NOT NULL,
    [CardId]    BIGINT NOT NULL,
    [LimitId]   BIGINT NOT NULL,
    CONSTRAINT [PK_BanlistCard] PRIMARY KEY CLUSTERED ([BanlistId] ASC, [CardId] ASC),
    CONSTRAINT [FK_BanlistCard_ToBanlist] FOREIGN KEY ([BanlistId]) REFERENCES [dbo].[Banlist] ([Id]) NOT FOR REPLICATION,
    CONSTRAINT [FK_BanlistCard_ToCard] FOREIGN KEY ([CardId]) REFERENCES [dbo].[Card] ([Id]) NOT FOR REPLICATION,
    CONSTRAINT [FK_BanlistCard_ToLimit] FOREIGN KEY ([LimitId]) REFERENCES [dbo].[Limit] ([Id]) NOT FOR REPLICATION
);

