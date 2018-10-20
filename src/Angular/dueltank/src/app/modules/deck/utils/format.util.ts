import {Format} from "../../../shared/models/format";
import {Card} from "../../../shared/models/card";
import {List} from "linqts";
import {BanlistCard} from "../../../shared/models/banlistcard";

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
