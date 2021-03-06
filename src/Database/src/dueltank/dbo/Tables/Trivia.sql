﻿CREATE TABLE [dbo].[Trivia] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [TriviaSectionId] BIGINT        NOT NULL,
    [Text]            VARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created]         DATETIME2 (7) NOT NULL,
    [Updated]         DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_Trivia] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Trivia_TriviaSection] FOREIGN KEY ([TriviaSectionId]) REFERENCES [dbo].[TriviaSection] ([Id]) NOT FOR REPLICATION
);

