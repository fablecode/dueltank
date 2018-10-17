import {Injectable} from "@angular/core";
import {Card} from "../../../shared/models/card";
import {Subject} from "rxjs";

@Injectable()
export class MainDeckService {

  // Observable Card sources
  private cardDropSuccessSource = new Subject<Card>();

  // Observable Card streams
  public cardDropSuccess$ = this.cardDropSuccessSource.asObservable();

  // Service message commands
  public cardDrop(card: Card) : void {
    this.cardDropSuccessSource.next(card);
  }
}
