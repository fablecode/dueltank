import {BanlistCard} from "./banlistcard";
import DateTimeFormat = Intl.DateTimeFormat;

export class LatestBanlist {
  public id: number;
  public name: string;
  public formatId: number;
  public releaseDate: DateTimeFormat;
  public cards: BanlistCard[];
}


