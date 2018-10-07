import {Component, Input} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";

@Component({
  selector: "deckTypes",
  templateUrl: "./deck-types.component.html"
})
export class DeckTypesComponent {
  @Input() deck: Deck;
}
