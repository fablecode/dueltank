import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "../../../shared/services/app-config.service";
import {Observable} from "rxjs";
import {Banlist} from "../../../shared/models/banlist";
import {MostRecentBanlistResult} from "../../../shared/models/most-recent-banlist-result";

@Injectable()
export class BanlistService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public latestBanlistByFormatAcronym(format: string): Observable<Banlist> {
    return this.http.get<Banlist>(this.configuration.apiEndpoint + "/api/banlists/" + format + "/latest");
  }

  public mostRecentBanlists() : Observable<MostRecentBanlistResult> {
    return this.http.get<MostRecentBanlistResult>(this.configuration.apiEndpoint + "/api/banlists/latest")
  }

}
