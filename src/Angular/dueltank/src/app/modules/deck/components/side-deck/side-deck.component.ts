import {Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {DragDropData} from "ng2-dnd";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {SideDeckService} from "../../services/side-deck.service";
import {Format} from "../../../../shared/models/format";
import {applyFormatToCards} from "../../utils/format.util";
import {
  fusionCardTypeCount,
  linkCardTypeCount,
  monsterCardTypeCount, spellCardTypeCount,
  synchroCardTypeCount, trapCardTypeCount,
  xyzCardTypeCount
} from "../../utils/deck.utils";
import {Subscription} from "rxjs";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";

@Component({
  selector: "sideDeck",
  templateUrl: "./side-deck.component.html"
})
export class SideDeckComponent implements OnChanges, OnInit, OnDestroy {
  @Input() cards: Card[] = [];

  public monsterCardCount: number = 0;
  public spellCardCount: number = 0;
  public trapCardCount: number = 0;
  public fusionCardCount: number = 0;
  public xyzCardCount: number = 0;
  public synchroCardCount: number = 0;
  public linkCardCount: number = 0;

  private currentFormat: Format;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private configuration: AppConfigService,
    private sideDeckService: SideDeckService,
    private deckCardFilterService : DeckCardFilterService,
    private deckCardSearchResultService: DeckCardSearchResultService
  ) {}


  public addToDeck($event: DragDropData) {
    this.sideDeckService.cardDrop($event.dragData)
  }

  public onRemoveCard(index: number) : boolean {
    this.sideDeckService.removeCard(index);
    return false;
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  public onCardHover(card: Card) {
    this.sideDeckService.cardHover(card);
  }

  ngOnInit(): void {
    let banListLoadedSubscription = this.deckCardFilterService.banlistLoadedSource$.subscribe( (format: Format) => {
      this.applyFormatToDeckCards(format);
    });

    let banListChangedSubscription = this.deckCardFilterService.banlistChangedSource$.subscribe( (format: Format) => {
      this.applyFormatToDeckCards(format);
    });

    // Add subscriptions to collection
    this.subscriptions = [...this.subscriptions, banListLoadedSubscription, banListChangedSubscription]
  }

  private applyFormatToDeckCards(format: Format) {
    applyFormatToCards(format, this.cards);
    this.currentFormat = format;
  }

  ngOnChanges(changes: SimpleChanges): void {
    for (let propName in changes) {
      if (propName === 'cards') {
        this.monsterCardCount = monsterCardTypeCount(this.cards)
        this.spellCardCount = spellCardTypeCount(this.cards)
        this.trapCardCount = trapCardTypeCount(this.cards)
        this.fusionCardCount = fusionCardTypeCount(this.cards)
        this.xyzCardCount = xyzCardTypeCount(this.cards)
        this.synchroCardCount = synchroCardTypeCount(this.cards)
        this.linkCardCount = linkCardTypeCount(this.cards)
      }
    }
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
