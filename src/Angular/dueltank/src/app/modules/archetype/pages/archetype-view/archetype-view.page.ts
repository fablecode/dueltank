import {Component, OnDestroy, OnInit, ViewEncapsulation} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {ArchetypeService} from "../../services/archetype.service";
import {Archetype} from "../../../../shared/models/archetype";
import {Subscription} from "rxjs";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {tap} from "rxjs/operators";
import {Card} from "../../../../shared/models/card";
import {forEach} from "@angular/router/src/utils/collection";
import {isSpellCard, isTrapCard} from "../../../deck/utils/card.util";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {CurrentHoverCardService} from "../../../../shared/services/current-hover-card.service";

@Component({
  templateUrl: "./archetype-view.page.html",
  styleUrls: ["./archetype-view.page.css"],
})
export class ArchetypeViewPage implements OnInit, OnDestroy {
  public isLoading: boolean = true;

  public selectedArchetype: Archetype;

  public monsters: Card[] = [];
  public spells: Card[] = [];
  public traps: Card[] = [];

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor
  (
    private activatedRoute: ActivatedRoute,
    private archetypeService: ArchetypeService,
    private seo: SearchEngineOptimizationService,
    private configuration: AppConfigService,
    private currentHoverCardService: CurrentHoverCardService
  ){}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.snapshot.params;

    let archetypeId = routeParams.id;
    let archetypeName = routeParams.name;

    this.seo.title(archetypeName + " - DuelTank");
    this.seo.keywords("View, Archetype, " + archetypeName + ", DuelTank")
    this.seo.robots("index,follow");

    let archetypeByIdSubscription = this.archetypeService
      .archetypeById(archetypeId)
      .pipe(
        tap(() => {this.isLoading = false;})
      )
      .subscribe((archetype: Archetype) => {

        this.selectedArchetype = archetype;

        for (let card of this.selectedArchetype.cards) {

          if(isSpellCard(card)) {
            this.spells = [...this.spells, card]
          }
          else if(isTrapCard(card)) {
            this.traps = [...this.traps, card];
          }
          else {
            this.monsters = [...this.monsters, card];
          }
        }
    });

    this.subscriptions = [...this.subscriptions, archetypeByIdSubscription];
  }

  public onCardHover(card: Card) {
    this.currentHoverCardService.cardChange(card);
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure not memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
