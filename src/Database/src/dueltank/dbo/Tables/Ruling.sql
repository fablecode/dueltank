CREATE TABLE [dbo].[Ruling] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RulingSectionId] BIGINT        NOT NULL,
    [Text]            VARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created]         DATETIME2 (7) NOT NULL,
    [Updated]         DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Ruling] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ruling_RulingSection] FOREIGN KEY ([RulingSectionId]) REFERENCES [dbo].[RulingSection] ([Id]) NOT FOR REPLICATION
);

