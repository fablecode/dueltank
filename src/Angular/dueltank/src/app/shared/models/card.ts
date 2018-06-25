import {Tip} from "./tip";

export class Card {
  public id: number = 0;
  public imageUrl: string;
  public name: string;
  public description: string;

  public tips: Tip[] = [];
}
