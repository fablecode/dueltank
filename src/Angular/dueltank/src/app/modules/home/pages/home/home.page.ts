import {Component, OnDestroy, OnInit} from "@angular/core";
import {DeckService} from "../../../../shared/services/deck.service";
import {Subscription} from "rxjs";
import {DeckSearchResult} from "../../../../shared/models/deck-search-result";
import {Deck} from "../../../../shared/models/deck";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {finalize, tap} from "rxjs/operators";
import {BanlistService} from "../../../banlist/services/banlist.service";
import {ArchetypeSearchResult} from "../../../../shared/models/archetype-search-result";
import {Archetype} from "../../../../shared/models/archetype";
import {MostRecentBanlistResult} from "../../../../shared/models/most-recent-banlist-result";
import {MostRecentBanlist} from "../../../../shared/models/most-recent-banlist";
import {ArchetypeService} from "../../../archetype/services/archetype.service";
import {Globals} from "../../../../globals";

@Component({
  templateUrl: './home.page.html'
})
export class HomePage implements OnInit, OnDestroy {
  public title: string = "Home";
  public isLoadingDecks: boolean = true;
  public isLoadingBanlist: boolean = true;
  public isLoadingArchetypes: boolean = true;
  public decks: Deck[] = [];
  public archetypes: Archetype[] = [];
  public banlists: MostRecentBanlist[] = [];

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor
  (
    private deckService: DeckService,
    private banlistService: BanlistService,
    private archetypeService: ArchetypeService,
    private configuration: AppConfigService,
    public globals: Globals
  ){}

  ngOnInit(): void {
    let mostRecentDecksSubscription = this.deckService
      .mostRecentDecks(8)
      .pipe(
        tap(() => this.isLoadingDecks = true),
        finalize(() => this.isLoadingDecks = false)
      )
      .subscribe((deckSearchResult: DeckSearchResult) => {
        this.decks = deckSearchResult.decks;
    });

    let mostRecentBanlistsSubscription = this.banlistService
      .mostRecentBanlists()
      .pipe(
        tap(() => this.isLoadingBanlist = true),
        finalize(() => this.isLoadingBanlist = false)
      )
      .subscribe((mostRecentBanlistResult: MostRecentBanlistResult) => {
        this.banlists = mostRecentBanlistResult.banlists;
    });

    let mostRecentArchetypesSubscription = this.archetypeService
      .mostRecentArchetypes(16)
      .pipe(
        tap(() => this.isLoadingArchetypes = true),
        finalize(() => {this.isLoadingArchetypes = false;})
      )
      .subscribe((archetypeSearchResult: ArchetypeSearchResult) => {
        this.archetypes = archetypeSearchResult.archetypes;
    });

    this.subscriptions =
      [...this.subscriptions,
        mostRecentDecksSubscription,
        mostRecentBanlistsSubscription,
        mostRecentArchetypesSubscription
      ];
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
