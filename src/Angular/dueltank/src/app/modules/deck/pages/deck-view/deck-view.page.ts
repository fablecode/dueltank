import {Component, OnDestroy, OnInit} from "@angular/core";
import {DeckService} from "../../../../shared/services/deck.service";
import {Deck} from "../../../../shared/models/deck";
import {ActivatedRoute} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {Subscription} from "rxjs";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {Card} from "../../../../shared/models/card";
import {MainDeckService} from "../../services/main-deck.service";
import {canAddCardToExtraDeck, canAddCardToMainDeck, canAddCardToSideDeck} from "../../utils/main-deck-rules.util";
import {Format} from "../../../../shared/models/format";
import {applyFormatToCards} from "../../utils/format.util";
import {ExtraDeckService} from "../../services/extra-deck.service";
import {SideDeckService} from "../../services/side-deck.service";

@Component({
  templateUrl: "./deck-view.page.html"
})
export class DeckViewPage implements OnInit, OnDestroy{
  public isLoading: boolean = true;
  public deckLoaded: boolean = false;
  public selectedDeck: Deck;

  private currentFormat: Format;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private deckService: DeckService,
    private activatedRoute: ActivatedRoute,
    private seo: SearchEngineOptimizationService,
    private deckCardFilterService : DeckCardFilterService,
    private deckCardSearchResultService: DeckCardSearchResultService,
    private mainDeckService: MainDeckService,
    private extraDeckService: ExtraDeckService,
    private sideDeckService: SideDeckService
  ){}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.snapshot.params;

    let deckName = routeParams.name;
    let deckId = routeParams.id;

    this.seo.title(deckName + " - DuelTank");
    this.seo.keywords("View, Deck," + deckName + ", DuelTank")
    this.seo.robots("index,follow");

    this.deckService.getDeckById(Number(deckId)).subscribe((deck: Deck) => {
      this.selectedDeck = deck;
      this.deckLoaded = true;
    });

    let cardFiltersLoadedSubscription = this.deckCardFilterService.cardFiltersLoaded$.subscribe(
      isLoaded => {
        this.isLoading = !isLoaded;
      }
    );

    // Subscriptions

    // main deck subscriptions
    let mainDeckCardDropSubscription = this.mainDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToMainDeck(this.selectedDeck, card, this.currentFormat)) {
        this.selectedDeck.mainDeck = [...this.selectedDeck.mainDeck, card]
      }
    });

    let removeMainDeckCardSubscription = this.mainDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.mainDeck.splice(index, 1);
    });

    // extra deck subscriptions
    let extraDeckCardDropSubscription = this.extraDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToExtraDeck(this.selectedDeck, card, this.currentFormat)) {
        this.selectedDeck.extraDeck = [...this.selectedDeck.extraDeck, card]
      }
    });

    let removeExtraDeckCardSubscription = this.extraDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.extraDeck.splice(index, 1);
    });

    // side deck subscriptions
    let sideDeckCardDropSubscription = this.sideDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToSideDeck(this.selectedDeck, card, this.currentFormat)) {
        this.selectedDeck.sideDeck = [...this.selectedDeck.sideDeck, card]
      }
    });

    let removeSideDeckCardSubscription = this.sideDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.sideDeck.splice(index, 1);
    });


    // Banlist
    let banListLoadedSubscription = this.deckCardFilterService.banlistLoadedSource$.subscribe( (format: Format) => {
      this.currentFormat = format;
    });

    let banListChangedSubscription = this.deckCardFilterService.banlistChangedSource$.subscribe( (format: Format) => {
      this.currentFormat = format;
    });

    // Add subscriptions to collection
    this.subscriptions = [
      ...this.subscriptions,
      cardFiltersLoadedSubscription,
      mainDeckCardDropSubscription,
      removeMainDeckCardSubscription,
      extraDeckCardDropSubscription,
      removeExtraDeckCardSubscription,
      sideDeckCardDropSubscription,
      removeSideDeckCardSubscription,
      banListLoadedSubscription,
      banListChangedSubscription
    ]
  }
  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
