import DateTimeFormat = Intl.DateTimeFormat;
import {Card} from "./card";

export class Archetype {
  public id: number;
  public name: string;
  public thumbnailUrl: string;
  public updated: DateTimeFormat;
  public totalCards: number;
  public cards: Card[];
}
