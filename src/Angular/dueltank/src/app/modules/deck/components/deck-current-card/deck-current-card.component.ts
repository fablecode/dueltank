import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {Card} from "../../../../shared/models/card";

@Component({
  selector: "deckCurrentCard",
  templateUrl: "./deck-current-card.component.html"
})
export class DeckCurrentCardComponent implements OnInit {
  @Input() card: Card;
  @Output() cardRightClick = new EventEmitter<Card>();

  public onCardRightClick(card: Card) {
    this.cardRightClick.emit(card);
  }

  ngOnInit(): void {
    if(!this.card) {
      this.card = new Card();
      this.card.imageUrl = "http://www.ygo-api.com/api/images/cards/no-card-image";
      this.card.name =
      this.card.description = "Hover over a card.";
    }
  }
}
