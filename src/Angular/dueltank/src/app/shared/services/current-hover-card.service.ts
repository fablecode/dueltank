import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {Card} from "../models/card";

@Injectable()
export class CurrentHoverCardService {
  // Observable card sources
  private cardChangeSource = new Subject<Card>();
  private currentCardRightClickSource = new Subject<Card>();

  // Observable card streams
  public cardChange$ = this.cardChangeSource.asObservable();
  public currentCardRightClick$ = this.currentCardRightClickSource.asObservable();

  public cardChange(card: Card) : void {
    this.cardChangeSource.next(card);
  }

  public rightClick(card: Card) : void {
    this.currentCardRightClickSource.next(card);
  }
}
