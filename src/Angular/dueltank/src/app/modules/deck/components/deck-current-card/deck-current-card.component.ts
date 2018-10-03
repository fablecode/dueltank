import {Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {Subscription} from "rxjs";
import {TipService} from "../../../../shared/services/tip.service";
import {TipSection} from "../../../../shared/models/tipSection";
import {TabsetComponent} from "ngx-bootstrap";

@Component({
  selector: "deckCurrentCard",
  templateUrl: "./deck-current-card.component.html"
})
export class DeckCurrentCardComponent implements OnInit, OnDestroy {
  public card: Card;
  @ViewChild('tabset') tabset: TabsetComponent;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private configuration: AppConfigService,
    private deckCardSearchResultService: DeckCardSearchResultService,
    private tipService: TipService
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
      this.tabset.tabs[0].active = true;
      this.card = card;
    });

    this.subscriptions.push(deckCardSearchResultSubscription)
  }

  public getApiEndPoint() {
    return this.configuration.apiEndpoint;
  }

  public changeTab(cardId: Number) {
    let tipSubscription = this.tipService.getTipsByCardId(cardId).subscribe((tips: TipSection[]) => {
      this.card.tipSections = tips;
    });

    this.subscriptions.push(tipSubscription);
  }

  public cardSearchByName(name: string) {
    
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe())
  }
}
