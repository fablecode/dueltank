﻿CREATE TABLE [dbo].[RulingSection] (
    [Id]      BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CardId]  BIGINT         NOT NULL,
    [Name]    NVARCHAR (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created] DATETIME2 (7)  NOT NULL,
    [Updated] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_RulingSection] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RulingSection_Card] FOREIGN KEY ([CardId]) REFERENCES [dbo].[Card] ([Id]) NOT FOR REPLICATION
);

