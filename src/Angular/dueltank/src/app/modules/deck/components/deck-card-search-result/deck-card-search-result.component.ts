import {Component, OnDestroy, OnInit} from "@angular/core";
import {CardSearchService} from "../../../../shared/services/cardSearch.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {DeckCardSearchModel} from "../../../../shared/models/forms/deck-card-search.model";
import {Subscription} from "rxjs";

@Component({
  selector: "deckCardSearchResult",
  templateUrl: "./deck-card-search-result.component.html"
})
export class DeckCardSearchResultComponent implements OnInit, OnDestroy {
  public totalCards : Number = 0;

  // Subscriptions
  private deckCardFilterServiceSubscription: Subscription;

  constructor(private cardSearchService: CardSearchService, private deckCardFilterService : DeckCardFilterService) {
    this.deckCardFilterServiceSubscription = this.deckCardFilterService.cardFiltersFormSubmittedSource$.subscribe( cardSearchCriteria => {
      console.log(cardSearchCriteria);
      this.Search(cardSearchCriteria);
    });
  }
  ngOnInit(): void {
  }

  private Search(cardSearchCriteria: DeckCardSearchModel) {
    this.cardSearchService.search(cardSearchCriteria).subscribe(result => {
      this.totalCards = result.totalRecords;
      console.log(result);
    })
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.deckCardFilterServiceSubscription.unsubscribe();
  }
}
