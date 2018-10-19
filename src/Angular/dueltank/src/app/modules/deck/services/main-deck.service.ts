import {Injectable} from "@angular/core";
import {Card} from "../../../shared/models/card";
import {Subject} from "rxjs";

@Injectable()
export class MainDeckService {

  // Observable Card sources
  private cardDropSuccessSource = new Subject<Card>();
  private cardRightClickSource = new Subject<number>();

  // Observable Card streams
  public cardDropSuccess$ = this.cardDropSuccessSource.asObservable();
  public cardRightClick$ = this.cardRightClickSource.asObservable();

  // Service message commands
  public cardDrop(card: Card) : void {
    this.cardDropSuccessSource.next(card);
  }

  public onRightClick(index: number) {
    this.cardRightClickSource.next(index);
  }
}
