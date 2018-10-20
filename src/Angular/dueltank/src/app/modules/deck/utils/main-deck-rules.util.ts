import {Card} from "../../../shared/models/card";
import {Deck} from "../../../shared/models/deck";
import {Format} from "../../../shared/models/format";
import {mainDeckAllowCardTypes, mainDeckSize} from "../../../shared/constants/main-deck.constants";
import {isCardTypeAllowed, onlyThreeCopiesOfTheSameCard} from "./deck-rules.util";
import {cardLimitInFormatNotReached} from "./format.util";

export function canAddCardToMainDeck(deck: Deck, card: Card, format: Format) : boolean {
  return deck.mainDeck.length < mainDeckSize &&
    isCardTypeAllowed(mainDeckAllowCardTypes, card) &&
    onlyThreeCopiesOfTheSameCard(deck, card) &&
    cardLimitInFormatNotReached(format, deck, card);
}
