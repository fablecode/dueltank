import {Card} from "../../../shared/models/card";
import {List} from "linqts";
import {AppConfigService} from "../../../shared/services/app-config.service";


export function cardTypeCount(cards: Card[], cardType: string) : number {
  let cardList = new List<Card>(cards);
  return cardList.Where(c => c.baseType === cardType).Count();
}

export function monsterCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, AppConfigService.monsterType);
}

export function spellCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, AppConfigService.spellType);
}

export function trapCardTypeCount(cards: Card[]) : number {
  return cardTypeCount(cards, AppConfigService.trapType);
}
