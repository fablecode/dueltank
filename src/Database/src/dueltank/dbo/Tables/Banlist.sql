CREATE TABLE [dbo].[Banlist] (
    [Id]          BIGINT         NOT NULL,
    [FormatId]    BIGINT         NOT NULL,
    [Name]        NVARCHAR (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [ReleaseDate] DATETIME       NOT NULL,
    [Created]     DATETIME2 (7)  NOT NULL,
    [Updated]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Banlists] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Banlist_Format] FOREIGN KEY ([FormatId]) REFERENCES [dbo].[Format] ([Id]) NOT FOR REPLICATION
);

