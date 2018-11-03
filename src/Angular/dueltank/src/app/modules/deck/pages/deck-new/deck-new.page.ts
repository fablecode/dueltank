import {Component, OnInit} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";

@Component({
  templateUrl: "./deck-new.page.html"
})
export class DeckNewPage implements OnInit {
  public isLoading: boolean = true;

  public newDeck: Deck = new Deck();

  ngOnInit(): void {
    this.isLoading = false;
  }
}
