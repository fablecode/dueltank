import {Component, OnDestroy, OnInit} from "@angular/core";
import {CardSearchService} from "../../../../shared/services/cardSearch.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {DeckCardSearchModel} from "../../../../shared/models/forms/deck-card-search.model";
import {Subscription} from "rxjs";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {Format} from "../../../../shared/models/format";
import {applyFormatToCards} from "../../utils/format.util";

@Component({
  selector: "deckCardSearchResult",
  templateUrl: "./deck-card-search-result.component.html"
})
export class DeckCardSearchResultComponent implements OnInit, OnDestroy {
  public totalCards : Number = 0;
  public cards: Card[] = [];
  public isLoadingCardResults: boolean = false;
  private currentFormat: Format;

  // Subscriptions
  private searchFormSubmittedSubscription: Subscription;
  private banlistLoadedSubscription: Subscription;
  private banlistChangedSubscription: Subscription;

  constructor(
    private cardSearchService: CardSearchService,
    private deckCardFilterService : DeckCardFilterService,
    private configuration: AppConfigService) {}

  ngOnInit(): void {
    // Subscribe to card search form
    this.searchFormSubmittedSubscription = this.deckCardFilterService.cardFiltersFormSubmittedSource$.subscribe( cardSearchCriteria => {
      this.Search(cardSearchCriteria);
    });

    this.banlistLoadedSubscription = this.deckCardFilterService.banlistLoadedSource$.subscribe( (format: Format) => {
      this.currentFormat = format;
    });

    this.banlistChangedSubscription = this.deckCardFilterService.banlistChangedSource$.subscribe( (format: Format) => {
      applyFormatToCards(format, this.cards);
      this.currentFormat = format;
    });
  }

  private Search(cardSearchCriteria: DeckCardSearchModel) {
    this.isLoadingCardResults = true;
    this.cardSearchService.search(cardSearchCriteria).subscribe(result => {
      this.totalCards = result.totalRecords;
      this.cards = result.cards;

      applyFormatToCards(this.currentFormat, this.cards)
      this.isLoadingCardResults = false;
    })
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.searchFormSubmittedSubscription.unsubscribe();
    this.banlistLoadedSubscription.unsubscribe();
    this.banlistChangedSubscription.unsubscribe();
  }
}
