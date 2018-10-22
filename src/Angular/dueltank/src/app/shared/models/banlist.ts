import {Card} from "./card";
import DateTimeFormat = Intl.DateTimeFormat;

export class Banlist {
  public format: string;
  public forbidden: Card[];
  public limited: Card[];
  public semiLimited: Card[];
  public unlimited: Card[];
  public releaseDate: DateTimeFormat;
}
