import {Component, Input} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {DeckService} from "../../../../shared/services/deck.service";

@Component({
  templateUrl: "./deck-view-card-search.component.html",
  selector: "deckViewCardSearch"
})
export class DeckViewCardSearchComponent {
  @Input() deck: Deck;

  constructor(private deckService: DeckService){}

  public downloadYdk(): void {
    this.deckService.downloadYdk(this.deck);
  }

  public deckToText() : string {
    return this.deckService.deckToText(this.deck);
  }

  public addDeckToUserFavourites() {

  }
}
