import {Component, OnDestroy, OnInit, ViewChild} from "@angular/core";
import {Card} from "../../models/card";
import {AppConfigService} from "../../services/app-config.service";
import {Subscription} from "rxjs";
import {TipService} from "../../services/tip.service";
import {TipSection} from "../../models/tipSection";
import {TabsetComponent} from "ngx-bootstrap";
import {CardSearchService} from "../../services/cardSearch.service";
import {HttpResponse} from "@angular/common/http";
import {ToastrService} from "ngx-toastr";
import {defaultDeckCurrentCard} from "../../../modules/deck/utils/card.util";
import {DeckCurrentCardTextService} from "../../../modules/deck/services/deck-current-card-text.service";
import {RulingService} from "../../services/ruling.service";
import {RulingSection} from "../../models/rulingSection";
import {CurrentHoverCardService} from "../../../modules/deck/services/current-hover-card.service";

@Component({
  selector: "currentHoverCard",
  templateUrl: "./current-hover-card.component.html"
})
export class CurrentHoverCardComponent implements OnInit, OnDestroy {
  public card: Card;
  @ViewChild('tabset') tabset: TabsetComponent;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private configuration: AppConfigService,
    private deckCurrentCardService: CurrentHoverCardService,
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

    let deckCurrentCardServiceSubscription = this.deckCurrentCardService.cardChange$.subscribe((card: Card) => {
      this.tabset.tabs[0].active = true;
      this.card = card;
    });

    let deckCurrentCardTextClickSubscription = this.deckCurrentCardTextService.currentCardTextClicked$.subscribe((cardName: string) => {
      this.cardSearchByName(cardName);
    });

    this.subscriptions.push(deckCurrentCardServiceSubscription);
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
