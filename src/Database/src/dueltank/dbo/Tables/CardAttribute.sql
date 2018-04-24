CREATE TABLE [dbo].[CardAttribute] (
    [AttributeId] BIGINT NOT NULL,
    [CardId]      BIGINT NOT NULL,
    CONSTRAINT [PK_CardAttributes] PRIMARY KEY CLUSTERED ([AttributeId] ASC, [CardId] ASC)
);

