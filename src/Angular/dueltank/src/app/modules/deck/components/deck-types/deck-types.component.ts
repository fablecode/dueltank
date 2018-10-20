import {Component, Input, OnDestroy, OnInit, ViewChild} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {TabDirective, TabsetComponent} from "ngx-bootstrap";
import {DeckCardSearchResultService} from "../../services/deck-card-search-result.service";
import {Card} from "../../../../shared/models/card";
import {List} from "linqts";
import {MainDeckService} from "../../services/main-deck.service";
import {ExtraDeckService} from "../../services/extra-deck.service";
import {SideDeckService} from "../../services/side-deck.service";
import {Subscription} from "rxjs";

@Component({
  selector: "deckTypes",
  templateUrl: "./deck-types.component.html"
})
export class DeckTypesComponent implements OnInit, OnDestroy {
  @Input() deck: Deck;
  @ViewChild('deckTabset') deckTabset: TabsetComponent;

  public activeTab: string;

  // Subscriptions
  private subscriptions: Subscription[] = [];

  constructor(
    private deckCardSearchResultService: DeckCardSearchResultService,
    private mainDeckService: MainDeckService,
    private extraDeckService: ExtraDeckService,
    private sideDeckService: SideDeckService,
  ){}

  ngOnInit(): void {
    // Subscriptions
    let deckCardSearchResultCardRightClickSubscription = this.deckCardSearchResultService.cardSearchResultCardRightClick$.subscribe((card : Card) => {

      let tabsList = new List<TabDirective>(this.deckTabset.tabs);

      const activeTab: TabDirective = tabsList.Single(t => t.active); //this.deckTabset.tabs.filter(tab => tab.active);

      switch (activeTab.heading) {
        case "Main":
          this.mainDeckService.cardDrop(card);
          break;
        case "Extra":
          this.extraDeckService.cardDrop(card);
          break;
        case "Side":
          this.sideDeckService.cardDrop(card);
          break;
        default: {
          throw new Error("Deck type '${activeTab.heading}' not found.");
        }
      }
    });

    // Add subscriptions to collection
    this.subscriptions = [...this.subscriptions, deckCardSearchResultCardRightClickSubscription]
  }

  ngOnDestroy(): void {
    // unsubscribe to ensure no memory leaks
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  public changeTab($event: TabDirective) {
    this.activeTab = $event.heading;

    console.log($event.heading);
  }
}
