import {Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {DragDropData} from "ng2-dnd";
import {Subscription} from "rxjs";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {ExtraDeckService} from "../../services/extra-deck.service";
import {
  fusionCardTypeCount, linkCardTypeCount,
  monsterCardTypeCount,
  spellCardTypeCount, synchroCardTypeCount,
  trapCardTypeCount,
  xyzCardTypeCount
} from "../../utils/deck.utils";
import {Format} from "../../../../shared/models/format";
import {applyFormatToCards} from "../../utils/format.util";

@Component({
  selector: "extraDeck",
  templateUrl: "./extra-deck.component.html"
})
export class ExtraDeckComponent implements OnChanges, OnInit, OnDestroy {
  @Input() cards: Card[] = [];

  private currentFormat: Format;
  public fusionCardCount: number = 0;
  public xyzCardCount: number = 0;
  public synchroCardCount: number = 0;
  public linkCardCount: number = 0;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private configuration: AppConfigService,
    private extraDeckService: ExtraDeckService,
    private deckCardFilterService : DeckCardFilterService
  ) {}


  public addToDeck($event: DragDropData) {
    this.extraDeckService.cardDrop($event.dragData)
  }

  public onRemoveCard(index: number) : boolean {
    this.extraDeckService.removeCard(index);
    return false;
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
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
