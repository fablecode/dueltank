CREATE TABLE [dbo].[CardLinkArrow] (
    [LinkArrowId] BIGINT NOT NULL,
    [CardId]      BIGINT NOT NULL,
    CONSTRAINT [PK_CardLinkArrow] PRIMARY KEY CLUSTERED ([LinkArrowId] ASC, [CardId] ASC)
);

