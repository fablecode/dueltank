CREATE TABLE [dbo].[SubCategory] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CategoryId] BIGINT         NOT NULL,
    [Name]       NVARCHAR (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created]    DATETIME2 (7)  NOT NULL,
    [Updated]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_SubCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

