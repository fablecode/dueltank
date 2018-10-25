import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {BanlistService} from "../../services/banlist.service";
import {Banlist} from "../../../../shared/models/banlist";
import {Card} from "../../../../shared/models/card";
import {MainDeckService} from "../../../deck/services/main-deck.service";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {CurrentHoverCardService} from "../../../deck/services/current-hover-card.service";

@Component({
  templateUrl: "./latest.page.html",
  styleUrls: ["./latest.page.css"]
})
export class LatestPage implements OnInit {

  public isLoading: boolean = true;
  public latestBanlist: Banlist;

  constructor(
    private activatedRoute: ActivatedRoute,
    private banlistService: BanlistService,
    private currentHoverCardService: CurrentHoverCardService,
    private configuration: AppConfigService
  ){}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.parent.snapshot.params;

    this.banlistService.latestBanlistByFormatAcronym(routeParams.format).subscribe((banlist: Banlist) => {
      this.latestBanlist = banlist;
      this.isLoading = false;
    });
  }

  public onCardHover(card: Card) {
    this.currentHoverCardService.cardChange(card);
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }
}
