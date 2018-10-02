import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {Subscription} from "rxjs";

@Component({
  selector: "deckCurrentCard",
  templateUrl: "./deck-current-card.component.html"
})
export class DeckCurrentCardComponent implements OnInit, OnDestroy {
  public card: Card;
  // @Output() cardRightClick = new EventEmitter<Card>();

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private configuration: AppConfigService,
    private deckCardSearchResultService: DeckCardSearchResultService
  ){}

  public onCardRightClick(card: Card) {
    // this.cardRightClick.emit(card);
  }

  ngOnInit(): void {
    if(!this.card) {
      this.card = new Card();
      this.card.imageUrl = this.configuration.apiEndpoint + "/api/images/cards/no-card-image";
      this.card.name =
      this.card.description = "Hover over a card.";
    }

    let deckCardSearchResultSubscription = this.deckCardSearchResultService.cardSearchResultCardHover$.subscribe((card: Card) => {
      this.card = card;
    });

    this.subscriptions.push(deckCardSearchResultSubscription)
  }

  public getApiEndPoint() {
    return this.configuration.apiEndpoint;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe())
  }
}
