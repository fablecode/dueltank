import {Card} from "../../../shared/models/card";
import {Deck} from "../../../shared/models/deck";
import {Format} from "../../../shared/models/format";
import {sideDeckAllowCardTypes, sideDeckSize } from "../../../shared/constants/main-deck.constants";
import {isCardTypeAllowed, onlyThreeCopiesOfTheSameCard} from "./deck-rules.util";
import {cardLimitInFormatNotReached} from "./format.util";

export function canAddCardToSideDeck(deck: Deck, card: Card, format: Format) : boolean {
  return deck.sideDeck.length < sideDeckSize &&
    isCardTypeAllowed(sideDeckAllowCardTypes, card) &&
    onlyThreeCopiesOfTheSameCard(deck, card) &&
    cardLimitInFormatNotReached(format, deck, card);
}
