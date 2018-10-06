import {Component, OnDestroy, OnInit, ViewChild} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {Subscription} from "rxjs";
import {TipService} from "../../../../shared/services/tip.service";
import {TipSection} from "../../../../shared/models/tipSection";
import {TabsetComponent} from "ngx-bootstrap";
import {CardSearchService} from "../../../../shared/services/cardSearch.service";
import {HttpResponse} from "@angular/common/http";
import {ToastrService} from "ngx-toastr";
import {defaultDeckCurrentCard} from "../../utils/card.util";
import {DeckCurrentCardTextService} from "../../services/deck-current-card-text.service";
import {RulingService} from "../../../../shared/services/ruling.service";
import {RulingSection} from "../../../../shared/models/rulingSection";

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
    private tipService: TipService,
    private rulingService: RulingService,
    private cardSearchService: CardSearchService,
    private toastr: ToastrService,
    private deckCurrentCardTextService : DeckCurrentCardTextService
  ){}

  public onCardRightClick(card: Card) {
    // this.cardRightClick.emit(card);
  }

  ngOnInit(): void {
    if(!this.card) {
      this.card = defaultDeckCurrentCard()
    }

    let deckCardSearchResultSubscription = this.deckCardSearchResultService.cardSearchResultCardHover$.subscribe((card: Card) => {
      this.tabset.tabs[0].active = true;
      this.card = card;
    });

    let deckCurrentCardTextClickSubscription = this.deckCurrentCardTextService.currentCardTextClicked$.subscribe((cardName: string) => {
      this.cardSearchByName(cardName);
    })

    this.subscriptions.push(deckCardSearchResultSubscription);
    this.subscriptions.push(deckCurrentCardTextClickSubscription);
  }

  public getApiEndPoint() {
    return this.configuration.apiEndpoint;
  }

  public changeTipTab(cardId: Number) {
    this.tipService.getTipsByCardId(cardId).subscribe((tips: TipSection[]) => {
      this.card.tipSections = tips;
    });
  }
  public changeRulingTab(cardId: Number) {
    this.rulingService.getRulingsByCardId(cardId).subscribe((rulings: RulingSection[]) => {
      this.card.rulingSections = rulings;
    });
  }

  public cardSearchByName(cardName: string) : void {
    this.cardSearchService.getCardByName(cardName).subscribe(
      (data: Card) => {
        this.tabset.tabs[0].active = true;
        this.card = data;
      },
      (error: HttpResponse<any>) => {
        if(error.status === 404) {
          this.toastr.warning("' " + cardName + " ' not found.", "Card Search");
        }
      });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe())
  }
}
