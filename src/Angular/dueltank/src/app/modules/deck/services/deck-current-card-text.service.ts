import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable()
export class DeckCurrentCardTextService {
  // Observable string sources
  private currentCardTextClicked = new Subject<string>()

  // Observable string streams
  public currentCardTextClicked$ = this.currentCardTextClicked.asObservable();

  // Service message commands
  public onCardTextClick(cardName: string) {
    this.currentCardTextClicked.next(cardName);
  }
}
