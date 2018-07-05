import {Component, OnInit} from "@angular/core";
import {DeckService} from "../../../../shared/services/deck.service";
import {Deck} from "../../../../shared/models/deck";
import {ActivatedRoute} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";

@Component({
  templateUrl: "./deck-view.page.html"
})
export class DeckViewPage implements OnInit{
  public selectedDeck: Deck;

  constructor(
    private deckService: DeckService,
    private activatedRoute: ActivatedRoute,
    private seo: SearchEngineOptimizationService
  ){}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.snapshot.params;
    var deckId = routeParams.id;
    var deckName = routeParams.name;

    this.seo.title(deckName + " - DuelTank");
    this.seo.keywords("View, Deck," + deckName + ", DuelTank")
    this.seo.robots("index,follow");

    this.deckService.getDeckById(deckId).subscribe(deck => {
      this.selectedDeck = deck;
    });
  }
}
