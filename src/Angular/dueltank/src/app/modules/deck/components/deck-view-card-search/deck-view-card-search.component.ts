import {Component, Input, OnInit} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {DeckService} from "../../../../shared/services/deck.service";
import {AppConfigService} from "../../../../shared/services/app-config.service";

@Component({
  templateUrl: "./deck-view-card-search.component.html",
  selector: "deckViewCardSearch"
})
export class DeckViewCardSearchComponent implements OnInit {
  @Input() deck: Deck;
  youtubeDefaultThumbnailUrl: string = "https://img.youtube.com/vi/default.jpg";

  constructor(private deckService: DeckService, public configuration: AppConfigService){}

  public downloadYdk(): void {
    this.deckService.downloadYdk(this.deck);
  }

  public deckToText() : string {
    return this.deckService.deckToText(this.deck);
  }

  public addDeckToUserFavourites() {

  }

  public goToUserDecks(username: string) {
  }

  public getYoutubeDefaultThumbnailUrl() {
    return "http://img.youtube.com/vi/" + this.deck.videoId + "/default.jpg";
  }

  ngOnInit(): void {
    this.youtubeDefaultThumbnailUrl = this.getYoutubeDefaultThumbnailUrl();
  }
}
