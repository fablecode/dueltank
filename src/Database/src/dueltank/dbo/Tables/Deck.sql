CREATE TABLE [dbo].[Deck] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserId]      NVARCHAR (450)  NOT NULL,
    [Name]        NVARCHAR (255)  NOT NULL,
    [Description] NVARCHAR (MAX)  NULL,
    [VideoUrl]    NVARCHAR (2083) NULL,
    [Created]     DATETIME2 (7)   NOT NULL,
    [Updated]     DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_Deck] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Deck_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

