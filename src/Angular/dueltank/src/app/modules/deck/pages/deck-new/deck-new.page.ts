import {Component, OnDestroy, OnInit} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {canAddCardToMainDeck} from "../../utils/main-deck-rules.util";
import {Card} from "../../../../shared/models/card";
import {MainDeckService} from "../../services/main-deck.service";
import {Subscription} from "rxjs";
import {Format} from "../../../../shared/models/format";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {CurrentHoverCardService} from "../../../../shared/services/current-hover-card.service";
import {canAddCardToExtraDeck} from "../../utils/extra-deck-rules.util";
import {canAddCardToSideDeck} from "../../utils/side-deck-rules.util";
import {DeckService} from "../../../../shared/services/deck.service";
import {ActivatedRoute} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {ExtraDeckService} from "../../services/extra-deck.service";
import {SideDeckService} from "../../services/side-deck.service";

@Component({
  templateUrl: "./deck-new.page.html"
})
export class DeckNewPage implements OnInit, OnDestroy {
  public isLoading: boolean = true;

  public newDeck: Deck = new Deck();

  // Subscriptions
  private subscriptions: Subscription[] = [];
  private currentFormat: Format;

  constructor
  (
    private deckService: DeckService,
    private activatedRoute: ActivatedRoute,
    private seo: SearchEngineOptimizationService,
    private deckCardFilterService : DeckCardFilterService,
    private deckCardSearchResultService: DeckCardSearchResultService,
    private mainDeckService: MainDeckService,
    private extraDeckService: ExtraDeckService,
    private sideDeckService: SideDeckService,
    private currentHoverCardService: CurrentHoverCardService
  ){}

  ngOnInit(): void {

    // Subscriptions

    // cards filters
    let cardFiltersLoadedSubscription = this.deckCardFilterService.cardFiltersLoaded$.subscribe(
      isLoaded => {
        this.isLoading = !isLoaded;
      }
    );

    // main deck subscriptions
    let mainDeckCardDropSubscription = this.mainDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToMainDeck(this.newDeck, card, this.currentFormat)) {
        this.newDeck.mainDeck = [...this.newDeck.mainDeck, card]
      }
    });

    let mainDeckCardHoverSubscription = this.mainDeckService.cardHoverSource$.subscribe((card: Card) => {
      this.currentHoverCardService.cardChange(card);
    });

    let removeMainDeckCardSubscription = this.mainDeckService.removeCard$.subscribe((index: number) => {
      this.newDeck.mainDeck.splice(index, 1);

      // https://stackoverflow.com/questions/42962394/angular-2-how-to-detect-changes-in-an-array-input-property
      // change the array reference and it will trigger the OnChanges mechanism
      this.newDeck.mainDeck = [].concat(this.newDeck.mainDeck);
    });

    // extra deck subscriptions
    let extraDeckCardDropSubscription = this.extraDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToExtraDeck(this.newDeck, card, this.currentFormat)) {
        this.newDeck.extraDeck = [...this.newDeck.extraDeck, card]
      }
    });

    let extraDeckCardHoverSubscription = this.extraDeckService.cardHoverSource$.subscribe((card: Card) => {
      this.currentHoverCardService.cardChange(card);
    });

    let removeExtraDeckCardSubscription = this.extraDeckService.removeCard$.subscribe((index: number) => {
      this.newDeck.extraDeck.splice(index, 1);

      // https://stackoverflow.com/questions/42962394/angular-2-how-to-detect-changes-in-an-array-input-property
      // change the array reference and it will trigger the OnChanges mechanism
      this.newDeck.extraDeck = [].concat(this.newDeck.extraDeck);
    });

    // side deck subscriptions
    let sideDeckCardDropSubscription = this.sideDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToSideDeck(this.newDeck, card, this.currentFormat)) {
        this.newDeck.sideDeck = [...this.newDeck.sideDeck, card]
      }
    });

    let sideDeckCardHoverSubscription = this.sideDeckService.cardHoverSource$.subscribe((card: Card) => {
      this.currentHoverCardService.cardChange(card);
    });


    let removeSideDeckCardSubscription = this.sideDeckService.removeCard$.subscribe((index: number) => {
      this.newDeck.sideDeck.splice(index, 1);

      // https://stackoverflow.com/questions/42962394/angular-2-how-to-detect-changes-in-an-array-input-property
      // change the array reference and it will trigger the OnChanges mechanism
      this.newDeck.sideDeck = [].concat(this.newDeck.sideDeck);
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
      mainDeckCardHoverSubscription,
      removeMainDeckCardSubscription,
      extraDeckCardDropSubscription,
      extraDeckCardHoverSubscription,
      removeExtraDeckCardSubscription,
      sideDeckCardDropSubscription,
      sideDeckCardHoverSubscription,
      removeSideDeckCardSubscription,
      banListLoadedSubscription,
      banListChangedSubscription
    ];
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
