import {Component, Input} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";

@Component({
  selector: "deckNewCardSearch",
  templateUrl: "./deck-new-card-search.component.html"
})
export class DeckNewCardSearchComponent {
  @Input() deck: Deck;
}
