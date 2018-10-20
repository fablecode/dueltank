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

@Component({
  templateUrl: "./deck-view.page.html"
})
export class DeckViewPage implements OnInit, OnDestroy{
  public isLoading: boolean = true;
  public deckLoaded: boolean = false;
  public selectedDeck: Deck;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private deckService: DeckService,
    private activatedRoute: ActivatedRoute,
    private seo: SearchEngineOptimizationService,
    private deckCardFilterService : DeckCardFilterService,
    private deckCardSearchResultService: DeckCardSearchResultService,
    private mainDeckService: MainDeckService
  )
  {
  }

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
    let deckCardSearchResultCardRightClickSubscription = this.deckCardSearchResultService.cardSearchResultCardRightClick$.subscribe((card : Card) => {
      this.selectedDeck.mainDeck = [...this.selectedDeck.mainDeck, card]
    });

    let mainDeckCardDropSubscription = this.mainDeckService.cardDropSuccess$.subscribe((card: Card) => {
      this.selectedDeck.mainDeck = [...this.selectedDeck.mainDeck, card]
    });

    let removeCardSubscription = this.mainDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.mainDeck.splice(index, 1);
    })

    // Add subscriptions to collection
    this.subscriptions = [
      ...this.subscriptions,
      deckCardSearchResultCardRightClickSubscription,
      mainDeckCardDropSubscription,
      cardFiltersLoadedSubscription,
      removeCardSubscription
    ]
  }
  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
