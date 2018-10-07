import {Component, Input, OnChanges, SimpleChanges} from "@angular/core";
import {Card} from "../../../../shared/models/card";

@Component({
  selector: "mainDeck",
  templateUrl: "./main-deck.component.html"
})
export class MainDeckComponent implements OnChanges {
  @Input() cards: Card[];

  public monsterCardCount: number = 0;
  public spellCardCount: number = 0;
  public trapCardCount: number = 0;

  ngOnChanges(changes: SimpleChanges): void {
    for (let propName in changes) {
      if (propName === 'cards') {
        
      }
    }
  }
}
