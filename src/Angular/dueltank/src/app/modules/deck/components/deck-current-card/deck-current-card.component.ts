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
    private cardSearchService: CardSearchService,
    private toastr: ToastrService
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

    this.subscriptions.push(deckCardSearchResultSubscription)
  }

  public getApiEndPoint() {
    return this.configuration.apiEndpoint;
  }

  public changeTab(cardId: Number) {
    this.tipService.getTipsByCardId(cardId).subscribe((tips: TipSection[]) => {
      this.card.tipSections = tips;
    });
  }

  public cardSearchByName(name: string) : void {
    this.cardSearchService.getCardByName(name).subscribe(
      (data: Card) => {
        this.card = data;
      },
      (error: HttpResponse<any>) => {
        if(error.status === 404) {
          this.toastr.warning("Card ",name);
        }
      });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe())
  }
}
