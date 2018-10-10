import {Component, Input, OnChanges, SimpleChanges} from "@angular/core";
import {Card} from "../../../../shared/models/card";
import {monsterCardTypeCount, spellCardTypeCount, trapCardTypeCount} from "../../utils/deck.utils";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";

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

  constructor(private configuration: AppConfigService) {}

  ngOnChanges(changes: SimpleChanges): void {
    for (let propName in changes) {
      if (propName === 'cards') {
        this.monsterCardCount = monsterCardTypeCount(this.cards)
        this.spellCardCount = spellCardTypeCount(this.cards)
        this.trapCardCount = trapCardTypeCount(this.cards)
      }
    }
  }

  public removeMovedItem(index: number, cardList: Card[]) {
    cardList.splice(index, 1);
  }

  drop(event: CdkDragDrop<Card[]>) {
    moveItemInArray(this.cards, event.previousIndex, event.currentIndex);
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }
}
