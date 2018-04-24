CREATE TABLE [dbo].[Limit] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]        NVARCHAR (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Description] NVARCHAR (MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    [Created]     DATETIME2 (7)  NOT NULL,
    [Updated]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Limit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

