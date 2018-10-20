import {Card} from "../../../shared/models/card";
import {List} from "linqts";
import {
  fusionType, linkType,
  monsterType,
  spellType,
  synchroType,
  trapType,
  xyzType
} from "../../../shared/constants/deck.constants";


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

export function fusionCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, fusionType);
}

export function xyzCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, xyzType);
}

export function synchroCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, synchroType);
}
export function linkCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, linkType);
}

export function cardCount(cards: Card[], card: Card) : number {
  let cardList = new List<Card>(cards);
  return cardList.Count(c => c.id === card.id);
}
