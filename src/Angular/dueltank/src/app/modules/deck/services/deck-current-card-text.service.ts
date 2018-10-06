import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable()
export class DeckCurrentCardTextService {
  // Observables
  private currentCardTextClicked = new Subject<string>()

  // Observable streams
  public currentCardTextClicked$ = this.currentCardTextClicked.asObservable();

  public onCardTextClick(cardName: string) {
    this.currentCardTextClicked.next(cardName);
  }
}
