import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from "@angular/core";
import {CardSearchService} from "../../../../shared/services/cardSearch.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {DeckCardSearchModel} from "../../../../shared/models/forms/deck-card-search.model";
import {Subscription} from "rxjs";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {Format} from "../../../../shared/models/format";
import {applyFormatToCards} from "../../utils/format.util";
import {CardSearchResult} from "../../../../shared/models/cardSearchResult.model";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";

@Component({
  selector: "deckCardSearchResult",
  templateUrl: "./deck-card-search-result.component.html"
})
export class DeckCardSearchResultComponent implements OnInit, OnDestroy {
  // fields
  public totalCards : Number = 0;
  public cards: Card[] = [];
  public isLoadingCardResults: boolean = false;
  private currentFormat: Format;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  // For scrolling purposes.
  @ViewChild('cardSearchResultsScroller') private cardSearchResultsContainer: ElementRef;

  constructor(
    private cardSearchService: CardSearchService,
    private deckCardFilterService : DeckCardFilterService,
    private configuration: AppConfigService,
    private deckCardSearchResultService: DeckCardSearchResultService
  ) {}

  ngOnInit(): void {
    // Subscriptions
    let searchFormSubmittedSubscription = this.deckCardFilterService.cardFiltersFormSubmittedSource$.subscribe( cardSearchCriteria => {
      if(cardSearchCriteria) {
        this.cardSearchResultsContainer.nativeElement.scrollTop = 0;
        this.Search(cardSearchCriteria);
      }
    });

    let banListLoadedSubscription = this.deckCardFilterService.banlistLoadedSource$.subscribe( (format: Format) => {
      this.currentFormat = format;
    });

    let banListChangedSubscription = this.deckCardFilterService.banlistChangedSource$.subscribe( (format: Format) => {
      applyFormatToCards(format, this.cards);
      this.currentFormat = format;
    });

    // Add subscriptions to collection
    this.subscriptions.push(searchFormSubmittedSubscription);
    this.subscriptions.push(banListLoadedSubscription);
    this.subscriptions.push(banListChangedSubscription);
  }

  private Search(cardSearchCriteria: DeckCardSearchModel) {
    this.isLoadingCardResults = true;
    this.cardSearchService.search(cardSearchCriteria)
      .subscribe((result: CardSearchResult) => {
        this.totalCards = result.totalRecords;
        if(cardSearchCriteria.pageIndex == 1) {
        this.cards = result.cards;
        }
        else {
          this.cards = this.cards.concat(result.cards);
        }

        applyFormatToCards(this.currentFormat, this.cards)
        this.isLoadingCardResults = false;
    })
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  public onScroll() {
    let latestCardSearch = this.deckCardFilterService.getLatestCardSearch();
    latestCardSearch.pageIndex += 1;
    this.Search(latestCardSearch);
  }

  public onCardHover(card: Card) {
    this.deckCardSearchResultService.onCardHover(card);
  }

  public removeMovedItem(index: number, cardList: Card[]) {
    cardList.splice(index, 1);
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
