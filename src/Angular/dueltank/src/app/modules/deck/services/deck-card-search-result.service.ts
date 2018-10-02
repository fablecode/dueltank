import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {Card} from "../../../shared/models/card";

@Injectable()
export class DeckCardSearchResultService {
  // Observables
  private cardSearchResultCardHover = new Subject<Card>()

  // Observable streams
  public cardSearchResultCardHover$ = this.cardSearchResultCardHover.asObservable();

  public onCardHover(card: Card) {
    this.cardSearchResultCardHover.next(card);
  }
}
