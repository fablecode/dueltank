import {Component, OnDestroy, OnInit} from "@angular/core";
import {tap} from "rxjs/operators";
import {DeckService} from "../../../../shared/services/deck.service";
import {DeckSearchResult} from "../../../../shared/models/deck-search-result";
import {Deck} from "../../../../shared/models/deck";
import {FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {Subscription} from "rxjs";
import {AppConfigService} from "../../../../shared/services/app-config.service";

@Component({
  templateUrl: "./deck-list.page.html"
})
export class DeckListPage implements OnInit, OnDestroy{
  public totalDecks: number;
  public decks: Deck[];
  public isLoading: boolean = true;

  public maxSize: number = 10;
  public currentPage = 1;
  public pageSize: number = 12;
  public rotate = true;

  // form
  public searchForm : FormGroup;
  public searchTermControl: FormControl = new FormControl(null);
  public searchTextControlSubscription: Subscription;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(private deckService: DeckService, private fb: FormBuilder, private configuration: AppConfigService){}

  ngOnInit(): void {
    this.searchForm = this.fb.group({
      searchTerm: this.searchTermControl
    });

    this.searchTextControlSubscription = this.searchTermControl
      .valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe((newValue: string) => {
        this.search(1, newValue)
      });

    this.search(1);

    this.subscriptions = [...this.subscriptions, this.searchTextControlSubscription]
  }

  public search(page: number, searchText: string = null) : void {
    this.deckService
      .search(searchText, this.pageSize, page)
      .pipe(
        tap(() => { this.isLoading = false;})
      )
      .subscribe((deckSearchResult: DeckSearchResult) => {
        this.totalDecks = deckSearchResult.totalDecks;
        this.decks = deckSearchResult.decks;
      });
  }

  public pageChanged(event: any): void {
    this.isLoading = true;
    this.search(event.page);
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
