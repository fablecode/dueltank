CREATE TABLE [dbo].[CardSubCategory] (
    [SubCategoryId] BIGINT NOT NULL,
    [CardId]        BIGINT NOT NULL,
    CONSTRAINT [PK_CardSubCategory] PRIMARY KEY CLUSTERED ([SubCategoryId] ASC, [CardId] ASC)
);

