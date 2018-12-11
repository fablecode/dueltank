import {Component, OnDestroy, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {DeckService} from "../../../../shared/services/deck.service";
import {Deck} from "../../../../shared/models/deck";
import {Card} from "../../../../shared/models/card";
import {canAddCardToMainDeck} from "../../utils/main-deck-rules.util";
import {canAddCardToExtraDeck} from "../../utils/extra-deck-rules.util";
import {canAddCardToSideDeck} from "../../utils/side-deck-rules.util";
import {Format} from "../../../../shared/models/format";
import {DeckEditorInfo} from "../../../../shared/models/deck-editor-info";
import {Observable, Subscription} from "rxjs";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {MainDeckService} from "../../services/main-deck.service";
import {ExtraDeckService} from "../../services/extra-deck.service";
import {SideDeckService} from "../../services/side-deck.service";
import {CurrentHoverCardService} from "../../../../shared/services/current-hover-card.service";
import {DeckInfoService} from "../../services/deck-info.service";
import {tap, mergeMap} from 'rxjs/operators';
import {UpdateDeckOutput} from "../../../../shared/models/decks/output/update-deck-output";
import {ToastrService} from "ngx-toastr";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  templateUrl: "./deck-editor.page.html"
})
export class DeckEditorPage implements OnInit, OnDestroy {
  public isLoading: boolean = true;
  public deckLoaded: boolean = false;
  public selectedDeck: Deck;

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
    private currentHoverCardService: CurrentHoverCardService,
    private deckInfoService: DeckInfoService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.snapshot.params;

    let deckName = routeParams.name;
    let deckId = routeParams.id;

    this.seo.robots("index,follow");

    this.deckService.getDeckById(Number(deckId))
      .pipe(
        tap(() => { this.isLoading = false;})
      )
      .subscribe((deck: Deck) => {
      this.selectedDeck = deck;

      this.seo.title(this.selectedDeck.name + " - DuelTank");
      this.seo.keywords("Editor, Deck," + deckName + ", DuelTank");

      this.deckLoaded = true;
    });

    // Subscriptions

    // cards filters
    let cardFiltersLoadedSubscription = this.deckCardFilterService.cardFiltersLoaded$.subscribe(
      isLoaded => {
        this.isLoading = !isLoaded;
      }
    );

    // main deck subscriptions
    let mainDeckCardDropSubscription = this.mainDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToMainDeck(this.selectedDeck, card, this.currentFormat)) {
        this.selectedDeck.mainDeck = [...this.selectedDeck.mainDeck, card]
      }
    });

    let mainDeckCardHoverSubscription = this.mainDeckService.cardHoverSource$.subscribe((card: Card) => {
      this.currentHoverCardService.cardChange(card);
    });

    let removeMainDeckCardSubscription = this.mainDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.mainDeck.splice(index, 1);

      // https://stackoverflow.com/questions/42962394/angular-2-how-to-detect-changes-in-an-array-input-property
      // change the array reference and it will trigger the OnChanges mechanism
      this.selectedDeck.mainDeck = [].concat(this.selectedDeck.mainDeck);
    });

    // extra deck subscriptions
    let extraDeckCardDropSubscription = this.extraDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToExtraDeck(this.selectedDeck, card, this.currentFormat)) {
        this.selectedDeck.extraDeck = [...this.selectedDeck.extraDeck, card]
      }
    });

    let extraDeckCardHoverSubscription = this.extraDeckService.cardHoverSource$.subscribe((card: Card) => {
      this.currentHoverCardService.cardChange(card);
    });

    let removeExtraDeckCardSubscription = this.extraDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.extraDeck.splice(index, 1);

      // https://stackoverflow.com/questions/42962394/angular-2-how-to-detect-changes-in-an-array-input-property
      // change the array reference and it will trigger the OnChanges mechanism
      this.selectedDeck.extraDeck = [].concat(this.selectedDeck.extraDeck);
    });

    // side deck subscriptions
    let sideDeckCardDropSubscription = this.sideDeckService.cardDropSuccess$.subscribe((card: Card) => {
      if(canAddCardToSideDeck(this.selectedDeck, card, this.currentFormat)) {
        this.selectedDeck.sideDeck = [...this.selectedDeck.sideDeck, card]
      }
    });

    let sideDeckCardHoverSubscription = this.sideDeckService.cardHoverSource$.subscribe((card: Card) => {
      this.currentHoverCardService.cardChange(card);
    });


    let removeSideDeckCardSubscription = this.sideDeckService.removeCard$.subscribe((index: number) => {
      this.selectedDeck.sideDeck.splice(index, 1);

      // https://stackoverflow.com/questions/42962394/angular-2-how-to-detect-changes-in-an-array-input-property
      // change the array reference and it will trigger the OnChanges mechanism
      this.selectedDeck.sideDeck = [].concat(this.selectedDeck.sideDeck);
    });


    // Banlist
    let banListLoadedSubscription = this.deckCardFilterService.banlistLoadedSource$.subscribe( (format: Format) => {
      this.currentFormat = format;
    });

    let banListChangedSubscription = this.deckCardFilterService.banlistChangedSource$.subscribe( (format: Format) => {
      this.currentFormat = format;
    });


    // deck info
    let deckInfoService = this.deckInfoService.saveDeck$.subscribe((deckEditorInfo: DeckEditorInfo) => {
      this.deckService
        .updateDeck(deckEditorInfo, this.selectedDeck)
        .pipe(
          mergeMap((updateDeckOutput : UpdateDeckOutput) => {

            if(deckEditorInfo.thumbnail) {
             return this.deckService.updateThumbnail(updateDeckOutput.deckId, deckEditorInfo.thumbnail)
            }

            return Observable.of(updateDeckOutput);
          })
        )
        .subscribe(() => {
          this.toastr.success(this.selectedDeck.name + " saved!.")
        },
        error => this.handleError(error));
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
      banListChangedSubscription,
      deckInfoService
    ];
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  handleError(httpError: HttpErrorResponse) : void {
    this.toastr.error(httpError.error[0]);
  }
}
