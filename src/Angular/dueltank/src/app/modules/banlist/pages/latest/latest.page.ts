import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {BanlistService} from "../../services/banlist.service";
import {Banlist} from "../../../../shared/models/banlist";

@Component({
  templateUrl: "./latest.page.html"
})
export class LatestPage implements OnInit {

  public isLoading: boolean = true;
  public latestBanlist: Banlist;

  constructor(
    private activatedRoute: ActivatedRoute,
    private banlistService: BanlistService
  ){}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.parent.snapshot.params;

    this.banlistService.latestBanlistByFormatAcronym(routeParams.format).subscribe((banlist: Banlist) => {
      this.latestBanlist = banlist;
      this.isLoading = false;
    });
  }
}
