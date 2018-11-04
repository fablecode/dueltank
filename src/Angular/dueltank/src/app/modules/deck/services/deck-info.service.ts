import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable()
export class DeckInfoService {
  // Observable sources
  private saveDeckSource = new Subject<DeckEditorInfo>();
  private shuffleDeckSource = new Subject<boolean>();
  private sortDeckSource = new Subject<boolean>();
  private clearDeckSource = new Subject<boolean>();

  // Observable string streams
  public saveDeck$ = this.saveDeckSource.asObservable();
  public shuffleDeck$ = this.shuffleDeckSource.asObservable();
  public sortDeck$ = this.sortDeckSource.asObservable();
  public clearDeck$ = this.clearDeckSource.asObservable();

  // Service message commands
  public saveDeck(deckEditorInfo: DeckEditorInfo) : void {
    this.saveDeckSource.next(deckEditorInfo);
  }

  public shuffleDeck() : void {
    this.shuffleDeckSource.next(true);
  }

  public sortDeck() : void {
    this.sortDeckSource.next(true);
  }

  public clearDeck() : void {
    this.shuffleDeckSource.next(true);
  }
}

export class DeckEditorInfo {
  public thumbnail: File;
  public name: string;
  public description: string;
  public videoUrl: string;
}
