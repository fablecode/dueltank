import {Component, OnInit} from "@angular/core";
import {DeckService} from "../../../../shared/services/deck.service";
import {Deck} from "../../../../shared/models/deck";
import {ActivatedRoute} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {forkJoin, Observable} from "rxjs";

@Component({
  templateUrl: "./deck-view.page.html"
})
export class DeckViewPage implements OnInit{
  public isLoading: boolean = true;
  public deckLoaded: boolean = false;
  public selectedDeck: Deck;
  public resolveData: any;
  constructor(
    private deckService: DeckService,
    private activatedRoute: ActivatedRoute,
    private seo: SearchEngineOptimizationService,
    private deckCardFilterService : DeckCardFilterService
  )
  {
    deckCardFilterService.cardFiltersLoaded$.subscribe(
      isLoaded => {
        console.log(isLoaded)
        this.isLoading = !isLoaded;
      }
    );
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
  }

  private getCardSearchResults() : Observable<any> {
    const routeParams = this.activatedRoute.snapshot.params;
    var deckId = routeParams.id;

    return forkJoin
    (
      this.deckService.getDeckById(Number(deckId))
    )
  }
}
