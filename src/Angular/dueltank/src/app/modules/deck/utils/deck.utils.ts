import {Card} from "../../../shared/models/card";
import {List} from "linqts";

export function monsterCardTypeCount(cards: Card[]) : number {
  return
}

export function cardTypeCount(cards: Card[], cardType: string) : number {
  let cardList = new List<Card>(cards);
  return cardList.Where(c => c.baseType === cardType).Count();
}
