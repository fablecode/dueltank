import {Component, Input, OnChanges, SimpleChanges} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {monsterCardTypeCount, spellCardTypeCount, trapCardTypeCount} from "../../utils/deck.utils";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {DragDropData} from "ng2-dnd";
import {MainDeckService} from "../../services/main-deck.service";

@Component({
  selector: "mainDeck",
  templateUrl: "./main-deck.component.html",
  styleUrls: ["./main-deck.component.css"]
})
export class MainDeckComponent implements OnChanges {
  @Input() cards: Card[] = [];
  public monsterCardCount: number = 0;
  public spellCardCount: number = 0;
  public trapCardCount: number = 0;

  constructor(private configuration: AppConfigService, private mainDeckService: MainDeckService) {}

  ngOnChanges(changes: SimpleChanges): void {
    for (let propName in changes) {
      if (propName === 'cards') {
        this.monsterCardCount = monsterCardTypeCount(this.cards)
        this.spellCardCount = spellCardTypeCount(this.cards)
        this.trapCardCount = trapCardTypeCount(this.cards)
      }
    }
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }

  public addToDeck($event: DragDropData) {
    this.mainDeckService.cardDrop($event.dragData)
  }

  public onRightClick(index: number) : void {
    this.mainDeckService.onRightClick(index);
  }
}
