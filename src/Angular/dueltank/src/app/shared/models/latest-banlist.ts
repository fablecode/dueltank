import {Card} from "./card";

export class LatestBanlist {
  public id: number;
  public name: string;
  public formatId: number;
  public releaseDate: DateTimeFormat;
  public cards: Card[];
}
