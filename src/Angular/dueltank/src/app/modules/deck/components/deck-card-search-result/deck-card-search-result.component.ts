import {Component, OnDestroy, OnInit} from "@angular/core";
import {CardSearchService} from "../../../../shared/services/cardSearch.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {DeckCardSearchModel} from "../../../../shared/models/forms/deck-card-search.model";
import {Subscription} from "rxjs";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";

@Component({
  selector: "deckCardSearchResult",
  templateUrl: "./deck-card-search-result.component.html"
})
export class DeckCardSearchResultComponent implements OnInit, OnDestroy {
  public totalCards : Number = 0;
  public cards: Card[] = [];
  public isLoadingCardResults: boolean = false;

  // Subscriptions
  private deckCardFilterServiceSubscription: Subscription;

  constructor(
    private cardSearchService: CardSearchService,
    private deckCardFilterService : DeckCardFilterService,
    private configuration: AppConfigService) {
    // Subscribe to card search form
    this.deckCardFilterServiceSubscription = this.deckCardFilterService.cardFiltersFormSubmittedSource$.subscribe( cardSearchCriteria => {
      this.Search(cardSearchCriteria);
    });
  }
  ngOnInit(): void {
  }

  private Search(cardSearchCriteria: DeckCardSearchModel) {
    this.isLoadingCardResults = true;
    this.cardSearchService.search(cardSearchCriteria).subscribe(result => {
      this.totalCards = result.totalRecords;
      this.cards = result.cards;

      this.isLoadingCardResults = false;
    })
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.deckCardFilterServiceSubscription.unsubscribe();
  }
}
