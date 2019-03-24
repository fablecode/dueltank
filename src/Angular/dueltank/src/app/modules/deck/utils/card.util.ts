import {Card} from "../../../shared/models/card";
import {Observable} from "rxjs";
import {monsterType, spellType, trapType} from "../../../shared/constants/deck.constants";

export function defaultDeckCurrentCard() : Card {
  let defaultCurrentCard = new Card;
  defaultCurrentCard.imageUrl = "/api/images/cards?name=no-card-image";
  defaultCurrentCard.name =
  defaultCurrentCard.description = "Hover over a card.";

  return defaultCurrentCard;
}

export function isMonsterCard(card: Card) : boolean {
  return card.baseType === monsterType;
}
export function isSpellCard(card: Card) : boolean {
  return card.baseType === spellType
}
export function isTrapCard(card: Card) : boolean {
  return card.baseType === trapType
}
