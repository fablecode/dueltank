CREATE PROCEDURE [dbo].[DeleteDeckById]
	@DeckId bigint = NULL,
	@UserId nvarchar(450)
AS
BEGIN
	SET @DeckId = (SELECT Id FROM dbo.Deck d WHERE d.Id = @DeckId AND d.UserId = @UserId)

	IF @DeckId IS NOT NULL
	BEGIN
		DELETE FROM dbo.DeckCard WHERE dbo.DeckCard.DeckId = @DeckId
		DELETE FROM dbo.Deck WHERE dbo.Deck.Id = @DeckId AND dbo.Deck.UserId = @UserId
	END

	SELECT @deckId
END