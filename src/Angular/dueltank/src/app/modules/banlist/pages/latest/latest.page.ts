import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {BanlistService} from "../../services/banlist.service";
import {Banlist} from "../../../../shared/models/banlist";

@Component({
  templateUrl: "./latest.page.html"
})
export class LatestPage implements OnInit {

  private isLoading: boolean = true;

  private format: string;
  private latestBanlist: Banlist;

  constructor(
    private activatedRoute: ActivatedRoute,
    private banlistService: BanlistService
  ){}

  ngOnInit(): void {
    const routeParams = this.activatedRoute.parent.snapshot.params;

    this.format = routeParams.format;

    this.banlistService.latestBanlistByFormatAcronym(this.format).subscribe((banlist: Banlist) => {
      this.latestBanlist = banlist;
      this.isLoading = false;
    });
  }
}
