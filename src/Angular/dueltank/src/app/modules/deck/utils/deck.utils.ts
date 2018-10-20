import {Card} from "../../../shared/models/card";
import {List} from "linqts";
import {monsterType, spellType, trapType} from "../../../shared/constants/deck.constants";


export function cardTypeCount(cards: Card[], cardType: string) : number {
  let cardList = new List<Card>(cards);
  return cardList.Count(c => c.baseType === cardType);
}

export function monsterCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, monsterType);
}

export function spellCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, spellType);
}

export function trapCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, trapType);
}

export function cardCount(cards: Card[], card: Card) : number {
  let cardList = new List<Card>(cards);
  return cardList.Count(c => c.id === card.id);
}
