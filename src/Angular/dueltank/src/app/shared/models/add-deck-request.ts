import {Card} from "./card";

export class AddDeckRequest {
  public name: string;
  public description: string;
  public videoUrl: string;
  public mainDeck: Card[];
  public extraDeck: Card[];
  public sideDeck: Card[];
}

