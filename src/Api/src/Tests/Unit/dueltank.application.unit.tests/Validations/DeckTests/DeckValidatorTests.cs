using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Deck;
using dueltank.application.Validations.Decks;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    //[TestFixture]
    //public class DeckValidatorTests
    //{
    //    private DeckValidator _sut;
    //    private DeckInputModel _inputModel;
    //    private Fixture _fixture;

    //    [SetUp]
    //    public void SetUp()
    //    {
    //        _sut = new DeckValidator();
    //        _inputModel = new DeckInputModel();
    //        _fixture = new Fixture();
    //    }

    //    [Test]
    //    public void Given_A_Deck_If_MoreThanThreeCopies_OfTheSameCard_InMainDeck_ExtraDeck_SideDeck_Combined_ValidationFails()
    //    {
    //        // Arrange
    //        const string expected = "You can only have up to 3 copies of the same card in your Main, Extra and Side Deck combined.";

    //        _inputModel.Name = "deck tester";
    //        _inputModel.UserId = Guid.NewGuid().ToString();

    //        _fixture.RepeatCount = 38;

    //        _inputModel.MainDeck =
    //                    _fixture
    //                        .Build<CardInputModel>()
    //                        .With(c => c.BaseType, "monster")
    //                        .Without(c => c.Types)
    //                        .CreateMany()
    //                        .ToList();

    //        _inputModel.MainDeck.AddRange(new List<CardInputModel>
    //        {
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "spell"
    //            },
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "spell"
    //            }
    //        });

    //        _inputModel.ExtraDeck = new List<CardInputModel>();

    //        _fixture.RepeatCount = 13;
    //        _inputModel.SideDeck =
    //                    _fixture
    //                        .Build<CardInputModel>()
    //                        .With(c => c.BaseType, "monster")
    //                        .Without(c => c.Types)
    //                        .CreateMany()
    //                        .ToList();

    //        _inputModel.SideDeck.AddRange(new List<CardInputModel>
    //        {
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "synchro"
    //            },
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "spell"
    //            }
    //        });

    //        // Act
    //        var result = _sut.Validate(_inputModel);

    //        // Assert
    //        result.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
    //    }

    //    [Test]
    //    public void Given_A_Deck_If_ThreeCopies_OfTheSameCard_InMainDeck_ExtraDeck_SideDeck_Combined_ValidationPass()
    //    {
    //        // Arrange
    //        _inputModel.Name = "deck tester";
    //        _inputModel.UserId = Guid.NewGuid().ToString();
    //        _fixture.RepeatCount = 38;
    //        _inputModel.MainDeck =
    //                        _fixture
    //                            .Build<CardInputModel>()
    //                            .With(c => c.BaseType, "monster")
    //                            .Without(c => c.Types)
    //                            .CreateMany()
    //                            .ToList();

    //        _inputModel.MainDeck.AddRange(new List<CardInputModel>
    //        {
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "spell"
    //            },
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "spell"
    //            }
    //        });

    //        _inputModel.ExtraDeck = new List<CardInputModel>();

    //        _fixture.RepeatCount = 14;
    //        _inputModel.SideDeck =
    //                        _fixture
    //                            .Build<CardInputModel>()
    //                            .With(c => c.BaseType, "monster")
    //                            .Without(c => c.Types)
    //                            .CreateMany()
    //                            .ToList();

    //        _inputModel.SideDeck.AddRange(new List<CardInputModel>
    //        {
    //            new CardInputModel
    //            {
    //                Id = 46,
    //                Name = "test card",
    //                BaseType = "spell"
    //            }
    //        });

    //        // Act
    //        var results = _sut.Validate(_inputModel);

    //        // Assert
    //        results.Errors.Should().BeEmpty();
    //    }
    //}

    //public class DeckValidator : AbstractValidator<DeckInputModel>
    //{
    //    public DeckValidator()
    //    {
    //        Include(new DeckNameValidator());
    //        Include(new YoutubeUrlValidator());
    //        Include(new MainDeckValidator());
    //        Include(new ExtraDeckValidator());
    //        Include(new SideDeckValidator());

    //        RuleFor(d => d)
    //            .Cascade(CascadeMode.StopOnFirstFailure)
    //            .Must(OnlyThreeCopiesOfTheSameCard)
    //            .WithMessage("You can only have up to 3 copies of the same card in your Main, Extra and Side Deck combined.");

    //        RuleFor(d => d.UserId).SetValidator(new UserIdValidator());
    //    }

    //    private static bool OnlyThreeCopiesOfTheSameCard(DeckInputModel deck)
    //    {
    //        var allCards =
    //            deck
    //                .MainDeck
    //                .Concat(deck.ExtraDeck)
    //                .Concat(deck.SideDeck)
    //                .ToList();

    //        var threeOrMoreCopies = allCards.GroupBy(c => c.Id).Count(x => x.Count() > 3);

    //        return threeOrMoreCopies == 0;
    //    }
    //}

    public class UserIdValidator : AbstractValidator<string>
    {
        public UserIdValidator()
        {
            RuleFor(id => id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("{PropertyName} should not be empty.")
                .Must(IsValid)
                .WithMessage("{PropertyName} not in the correct format.")
                .WithName("UserId");
        }

        private bool IsValid(string userId)
        {
            return Guid.TryParse(userId, out _);
        }
    }

    [TestFixture]
    public class UserIdValidatorTests
    {
        private UserIdValidator _sut;
        private string _inputModel;

        [SetUp]
        public void SetUp()
        {
            _sut = new UserIdValidator();
            _inputModel = null;

        }

        [Test]
        public void Given_A_Null_UserId_Then_Validation_StopOnFirstFailure()
        {
            // Arrange

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().HaveCount(1);
        }

        [Test]
        public void Given_A_Empty_UserId_Then_Validation_StopOnFirstFailure()
        {
            // Arrange
            _inputModel = string.Empty;

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().HaveCount(1);
        }

        [Test]
        public void Given_An_Empty_UserId_Then_Should_Return_Empty_ErrorMessage()
        {
            // Arrange
            const string expected = "UserId should not be empty.";

            _inputModel = string.Empty;

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_An_Invalid_UserId_Then_Validation_Should_Fail()
        {
            // Arrange
            const string expected = "UserId not in the correct format.";
            _inputModel = "sdfjsafhasjdfaisd-sdfs";

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_A_Valid_UserId_Then_Validation_Should_Pass()
        {
            // Arrange
            _inputModel = Guid.NewGuid().ToString();

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().BeEmpty();
        }
    }

    public static class UserValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> UserIdValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotNull()
                .SetValidator(new UserIdValidator());
        }

        //public static IRuleBuilderOptions<T, string> UserNameValidator<T>(this IRuleBuilder<T, string> rule)
        //{
        //    return rule
        //        .NotNull()
        //        .SetValidator(new UserNameValidator());
        //}
    }
}