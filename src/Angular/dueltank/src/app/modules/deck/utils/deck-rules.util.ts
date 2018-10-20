import {Deck} from "../../../shared/models/deck";
import {Card} from "../../../shared/models/card";
import {List} from "linqts";
import {maxCardCopies} from "../../../shared/constants/deck.constants";

export function onlyThreeCopiesOfTheSameCard(deck: Deck, card: Card) : boolean {
  let mainDeckCardList = new List<Card>(deck.mainDeck);
  let extraDeckCardList = new List<Card>(deck.extraDeck);
  let sideDeckCardList = new List<Card>(deck.sideDeck);

  return mainDeckCardList.Count(c => c.id === card.id) +
         extraDeckCardList.Count(c => c.id === card.id) +
         sideDeckCardList.Count(c => c.id === card.id) < maxCardCopies
}

export function isCardTypeAllowed(allowedCardTypes: string[], card: Card) : boolean {
  let cardTypes = new List<string>(allowedCardTypes);
  return cardTypes.Any(c => c === card.baseType);
}
