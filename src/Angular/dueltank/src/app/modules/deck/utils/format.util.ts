import {Format} from "../../../shared/models/format";
import {Card} from "../../../shared/models/card";
import {List} from "linqts";
import {BanlistCard} from "../../../shared/models/banlistcard";
import {Deck} from "../../../shared/models/deck";
import {cardCount} from "./deck.utils";

export const FORBIDDEN : string = "forbidden";
export const LIMITED : string = "limited";
export const SEMI_LIMITED : string = "semi-limited";
export const UNLIMITED : string = "unlimited";

export const limits: { [index: string]: number; } = {
  "forbidden": 0,
  "limited": 1,
  "semi-limited": 2,
  "unlimited": 3
};

export function applyFormatToCards(format : Format, cards: Card[]) {
  let cardList = new List<Card>(cards);
  let banlistCards = new List<BanlistCard>(format.latestBanlist.cards);

  cardList.ForEach(card => {
    let banlistCard = banlistCards.SingleOrDefault(blc => blc.banlistId === format.latestBanlist.id && blc.cardId === card.id);
    if(banlistCard) {
      card.limit = banlistCard.limit;
    }
  });
}

export function applyFormatToCard(format : Format, card: Card) {
  let banlistCards = new List<BanlistCard>(format.latestBanlist.cards);
  let banlistCard = banlistCards.SingleOrDefault(blc => blc.banlistId === format.latestBanlist.id && blc.cardId === card.id);
  if(banlistCard) {
    card.limit = banlistCard.limit;
  }
}

export function numberOfCopiesAllowedInFormat(format: Format, card: Card) {
  let cards = new List<BanlistCard>(format.latestBanlist.cards);

  let banlistCard = cards.SingleOrDefault(bc => bc.banlistId === format.latestBanlist.id && bc.cardId == card.id);

  if(banlistCard) {
    switch (banlistCard.limit) {
      case FORBIDDEN:
        return limits[FORBIDDEN];
      case LIMITED:
        return limits[LIMITED];
      case SEMI_LIMITED:
        return limits[SEMI_LIMITED];
    }
  }

  return limits[UNLIMITED];
}

export function cardLimitInFormatNotReached(format:Format, deck: Deck, card: Card) : boolean {
  let mainDeckCardCount = cardCount(deck.mainDeck, card);
  let extraDeckCardCount = cardCount(deck.extraDeck, card);
  let sideDeckCardCount = cardCount(deck.sideDeck, card);

  let numberOfCardCopies = mainDeckCardCount + extraDeckCardCount + sideDeckCardCount;

  let numberOfCardCopiesAllowed = numberOfCopiesAllowedInFormat(format, card);

  return numberOfCardCopies < numberOfCardCopiesAllowed
}
