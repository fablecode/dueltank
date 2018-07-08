import {Component, Input, OnInit, TemplateRef} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {DeckService} from "../../../../shared/services/deck.service";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {BsModalRef, BsModalService} from "ngx-bootstrap";

@Component({
  templateUrl: "./deck-view-card-search.component.html",
  selector: "deckViewCardSearch"
})
export class DeckViewCardSearchComponent implements OnInit {
  @Input() deck: Deck;
  youtubeDefaultThumbnailUrl: string = "https://img.youtube.com/vi/default.jpg";
  modalRef: BsModalRef;
  player: YT.Player;
  private id: string;

  constructor
  (
    private deckService: DeckService,
    public configuration: AppConfigService,
    private modalService: BsModalService
  ){}

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

  public openYoutubeModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, Object.assign({}, { class: 'gray modal-lg' }));
  }

  public savePlayer(player) {
    this.player = player;
    this.playVideo();
  }

  public playVideo() {
    this.player.playVideo();
  }

  ngOnInit(): void {
    this.youtubeDefaultThumbnailUrl = this.getYoutubeDefaultThumbnailUrl();
  }
}
