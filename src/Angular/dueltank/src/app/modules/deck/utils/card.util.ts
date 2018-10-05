import {Card} from "../../../shared/models/card";

export function defaultDeckCurrentCard() : Card {
  let defaultCurrentCard = new Card;
  defaultCurrentCard.imageUrl = "/api/images/cards/no-card-image";
  defaultCurrentCard.name =
  defaultCurrentCard.description = "Hover over a card.";

  return defaultCurrentCard;
}
