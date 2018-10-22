import {Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {monsterCardTypeCount, spellCardTypeCount, trapCardTypeCount} from "../../utils/deck.utils";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DragDropData} from "ng2-dnd";
import {MainDeckService} from "../../services/main-deck.service";
import {Format} from "../../../../shared/models/format";
import {applyFormatToCards} from "../../utils/format.util";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {Subscription} from "rxjs";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {DeckCurrentCardService} from "../../services/deck-current-card.service";

@Component({
  selector: "mainDeck",
  templateUrl: "./main-deck.component.html",
  styleUrls: ["./main-deck.component.css"]
})
export class MainDeckComponent implements OnChanges, OnInit, OnDestroy {
  @Input() cards: Card[] = [];

  private currentFormat: Format;
  public monsterCardCount: number = 0;
  public spellCardCount: number = 0;
  public trapCardCount: number = 0;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private configuration: AppConfigService,
    private mainDeckService: MainDeckService,
    private deckCardFilterService : DeckCardFilterService,
    private deckCurrentCardService: DeckCurrentCardService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    for (let propName in changes) {
      if (propName === 'cards') {
        this.monsterCardCount = monsterCardTypeCount(this.cards)
        this.spellCardCount = spellCardTypeCount(this.cards)
        this.trapCardCount = trapCardTypeCount(this.cards)
      }
    }
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  public addToDeck($event: DragDropData) {
    this.mainDeckService.cardDrop($event.dragData)
  }

  public onRemoveCard(index: number) : boolean {
    this.mainDeckService.removeCard(index);
    return false;
  }

  public onCardHover(card: Card) {
    this.deckCurrentCardService.cardChange(card);
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

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
