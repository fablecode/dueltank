import {Injectable} from "@angular/core";
import {Card} from "../../../shared/models/card";
import {Subject} from "rxjs";

@Injectable()
export class SideDeckService {

  // Observable Card sources
  private cardDropSuccessSource = new Subject<Card>();
  private cardHoverSource = new Subject<Card>();
  private removeCardSource = new Subject<number>();

  // Observable Card streams
  public cardDropSuccess$ = this.cardDropSuccessSource.asObservable();
  public cardHoverSource$ = this.cardHoverSource.asObservable();
  public removeCard$ = this.removeCardSource.asObservable();

  // Service message commands
  public cardDrop(card: Card) : void {
    this.cardDropSuccessSource.next(card);
  }

  public cardHover(card: Card) : void {
    this.cardHoverSource.next(card);
  }

  public removeCard(index: number) {
    this.removeCardSource.next(index);
  }
}
