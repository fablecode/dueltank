import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import { AppConfigService} from "./app-config.service";
import {UserProfile} from "../models/userprofile";
import {HttpClient} from "@angular/common/http";

@Injectable()
export class AccountsService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public Profile() : Observable<UserProfile> {
    // Make the API call using the new parameters.
    return this.http.get<UserProfile>(this.configuration.apiEndpoint + "/api/accounts/profile");
  }
}
