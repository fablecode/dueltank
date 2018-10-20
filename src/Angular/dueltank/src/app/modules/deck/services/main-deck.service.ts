import {Injectable} from "@angular/core";
import {Card} from "../../../shared/models/card";
import {Subject} from "rxjs";

@Injectable()
export class MainDeckService {

  // Observable Card sources
  private cardDropSuccessSource = new Subject<Card>();
  private removeCardSource = new Subject<number>();

  // Observable Card streams
  public cardDropSuccess$ = this.cardDropSuccessSource.asObservable();
  public removeCard$ = this.removeCardSource.asObservable();

  // Service message commands
  public cardDrop(card: Card) : void {
    this.cardDropSuccessSource.next(card);
  }

  public removeCard(index: number) {
    this.removeCardSource.next(index);
  }
}

@Injectable()
export class DeckRulesService {
  private onlyThreeCopiesOfTheSameCard
}
