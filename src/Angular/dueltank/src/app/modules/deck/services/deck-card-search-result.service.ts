import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {Card} from "../../../shared/models/card";

@Injectable()
export class DeckCardSearchResultService {
  // Observable card sources
  private cardSearchResultCardHover = new Subject<Card>();
  private cardSearchResultCardRightClick = new Subject<Card>();

  // Observable card streams
  public cardSearchResultCardHover$ = this.cardSearchResultCardHover.asObservable();
  public cardSearchResultCardRightClick$ = this.cardSearchResultCardRightClick.asObservable();

  // Service message commands
  public onCardHover(card: Card) : void {
    this.cardSearchResultCardHover.next(card);
  }

  public onRightClick(card: Card) : void {
    this.cardSearchResultCardRightClick.next(card);
  }
}


