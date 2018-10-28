import {Component, OnInit, ViewChild, ViewEncapsulation} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {BanlistService} from "../../services/banlist.service";
import {Banlist} from "../../../../shared/models/banlist";
import {Card} from "../../../../shared/models/card";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {CurrentHoverCardService} from "../../../deck/services/current-hover-card.service";
import {tap} from "rxjs/operators";
import {TabsetComponent} from "ngx-bootstrap";

@Component({
  templateUrl: "./latest.page.html",
  styleUrls: ["./latest.page.css"],
  encapsulation: ViewEncapsulation.None
})
export class LatestPage implements OnInit {

  public isLoading: boolean = true;
  public latestBanlist: Banlist;
  @ViewChild('banlistTabset') tabset: TabsetComponent;

  constructor(
    private activatedRoute: ActivatedRoute,
    private banlistService: BanlistService,
    private currentHoverCardService: CurrentHoverCardService,
    private configuration: AppConfigService
  ){}

  ngOnInit(): void {

    // Force reload when navigating to same route with different params
    // https://stackoverflow.com/questions/47808717/how-to-update-the-same-component-with-different-params-in-angular
    this.activatedRoute.parent.params.subscribe(params => {
      this.isLoading = true;
      this.banlistService.latestBanlistByFormatAcronym(params.format)
        .pipe(
          tap(() => {
            this.tabset.tabs[0].active = true;
            this.isLoading = false;
          })
        )
        .subscribe((banlist: Banlist) => {
          this.latestBanlist = banlist;
      });
    });
  }

  public onCardHover(card: Card) {
    this.currentHoverCardService.cardChange(card);
  }

  public getApiEndPointUrl() : string {
    return this.configuration.apiEndpoint;
  }
}
