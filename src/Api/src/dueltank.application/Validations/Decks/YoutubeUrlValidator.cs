using System.Text.RegularExpressions;
using dueltank.application.Models.Decks.Input;
using FluentValidation;

namespace dueltank.application.Validations.Decks
{
    public class YoutubeUrlValidator : AbstractValidator<DeckInputModel>
    {
        public const string UrlRegexExpression =
            @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^""&?\/ ]{11})";

        public YoutubeUrlValidator()
        {
            RuleFor(d => d.VideoUrl)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Matches(UrlRegexExpression)
                .Must(ContainsYoutubeVideoId)
                .When(d => !string.IsNullOrWhiteSpace(d.VideoUrl));
        }

        private static bool ContainsYoutubeVideoId(string videoUrl)
        {
            var idMatch = new Regex(UrlRegexExpression).Match(videoUrl);

            return !string.IsNullOrWhiteSpace(idMatch.Groups[1].Value);
        }
    }
}