﻿CREATE TABLE [dbo].[Card] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CardNumber]  BIGINT         NULL,
    [Name]        NVARCHAR (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Description] NVARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [CardLevel]   INT            NULL,
    [CardRank]    INT            NULL,
    [Atk]         INT            NULL,
    [Def]         INT            NULL,
    [Created]     DATETIME2 (7)  NOT NULL,
    [Updated]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Card] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
CREATE FULLTEXT INDEX ON [dbo].[Card]
    ([Name] LANGUAGE 1033, [Description] LANGUAGE 1033)
    KEY INDEX [PK_Card]
    ON [CardCatalog];

