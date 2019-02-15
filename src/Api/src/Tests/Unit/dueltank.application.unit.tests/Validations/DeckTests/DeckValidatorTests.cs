using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using dueltank.application.Models.Cards.Input;
using dueltank.application.Models.Decks.Input;
using dueltank.application.Validations.Decks;
using dueltank.tests.core;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace dueltank.application.unit.tests.Validations.DeckTests
{
    [TestFixture]
    [Category(TestType.Unit)]
    public class DeckValidatorTests
    {
        private DeckValidator _sut;
        private DeckInputModel _inputModel;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _sut = new DeckValidator();
            _inputModel = new DeckInputModel();
            _fixture = new Fixture();
        }

        [Test]
        public void Given_A_Deck_If_More_Than_Three_Copies_Of_The_Same_Card_In_MainDeck_ExtraDeck_SideDeck_Combined_ValidationFails()
        {
            // Arrange
            const string expected = "You can only have up to 3 copies of the same card in your Main, Extra and Side Deck combined.";

            _inputModel.Name = "deck tester";
            _inputModel.UserId = Guid.NewGuid().ToString();

            _fixture.RepeatCount = 38;

            _inputModel.MainDeck =
                        _fixture
                            .Build<CardInputModel>()
                            .With(c => c.BaseType, "monster")
                            .With(c => c.Id, new Random().Next(int.MaxValue))
                            .Without(c => c.Types)
                            .CreateMany()
                            .ToList();

            _inputModel.MainDeck.AddRange(new List<CardInputModel>
            {
                new CardInputModel
                {
                    Id = 46,
                    Name = "test card",
                    BaseType = "spell"
                },
                new CardInputModel
                {
                    Id = 46,
                    Name = "test card",
                    BaseType = "spell"
                }
            });

            _inputModel.ExtraDeck = new List<CardInputModel>();

            _fixture.RepeatCount = 13;
            _inputModel.SideDeck =
                        _fixture
                            .Build<CardInputModel>()
                            .With(c => c.BaseType, "monster")
                            .Without(c => c.Types)
                            .CreateMany()
                            .ToList();

            _inputModel.SideDeck.AddRange(new List<CardInputModel>
            {
                new CardInputModel
                {
                    Id = 46,
                    Name = "test card",
                    BaseType = "synchro"
                },
                new CardInputModel
                {
                    Id = 46,
                    Name = "test card",
                    BaseType = "spell"
                }
            });

            // Act
            var result = _sut.Validate(_inputModel);

            // Assert
            result.Errors.Should().ContainSingle(err => err.ErrorMessage == expected);
        }

        [Test]
        public void Given_A_Deck_If_ThreeCopies_Of_The_SameCard_In_MainDeck_ExtraDeck_SideDeck_Combined_ValidationPass()
        {
            // Arrange
            _inputModel.Name = "deck tester";
            _inputModel.UserId = Guid.NewGuid().ToString();
            _fixture.RepeatCount = 38;
            _inputModel.MainDeck =
                            _fixture
                                .Build<CardInputModel>()
                                .With(c => c.BaseType, "monster")
                                .Without(c => c.Types)
                                .CreateMany()
                                .ToList();

            _inputModel.MainDeck.AddRange(new List<CardInputModel>
            {
                new CardInputModel
                {
                    Id = long.MaxValue,
                    Name = "test card",
                    BaseType = "spell"
                },
                new CardInputModel
                {
                    Id = long.MaxValue,
                    Name = "test card",
                    BaseType = "spell"
                }
            });

            _inputModel.ExtraDeck = new List<CardInputModel>();

            _fixture.RepeatCount = 14;
            _inputModel.SideDeck =
                            _fixture
                                .Build<CardInputModel>()
                                .With(c => c.BaseType, "monster")
                                .Without(c => c.Types)
                                .CreateMany()
                                .ToList();

            _inputModel.SideDeck.AddRange(new List<CardInputModel>
            {
                new CardInputModel
                {
                    Id = long.MaxValue,
                    Name = "test card",
                    BaseType = "spell"
                }
            });

            // Act
            var results = _sut.Validate(_inputModel);

            // Assert
            results.Errors.Should().BeEmpty();
        }

        [Test]
        public void Given_A_New_Deck_If_Id_HasValue_Validation_Should_Fail()
        {
            // Arrange
            _inputModel.Id = 89798789;
            _inputModel.MainDeck = new List<CardInputModel>();
            _inputModel.ExtraDeck = new List<CardInputModel>();
            _inputModel.SideDeck = new List<CardInputModel>();

            // Act
            var result = _sut.ShouldHaveValidationErrorFor(deck => deck.Id, _inputModel, DeckValidator.InsertDeckRuleSet);

            // Assert
            result.Should().ContainSingle(err => err.ErrorMessage == "Deck Id cannot have a value when creating a deck.");
        }

        [Test]
        public void Given_A_Deck_When_Updating_The_Deck_If_Id_HasNotValue_ValidationFail()
        {
            // Arrange
            _inputModel.Id = null;
            _inputModel.MainDeck = new List<CardInputModel>();
            _inputModel.ExtraDeck = new List<CardInputModel>();
            _inputModel.SideDeck = new List<CardInputModel>();

            // Act
            var result = _sut.ShouldHaveValidationErrorFor(deck => deck.Id, _inputModel, DeckValidator.UpdateDeckRuleSet);

            // Assert
            result.Should().ContainSingle(err => err.ErrorMessage == "Deck Id must have a value when updating a deck.");
        }

    }
}