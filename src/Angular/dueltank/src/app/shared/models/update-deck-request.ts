import {Card} from "./card";

export class UpdateDeckRequest {
  public id: number;
  public name: string;
  public description: string;
  public videoUrl: string;
  public mainDeck: Card[];
  public extraDeck: Card[];
  public sideDeck: Card[];
}
