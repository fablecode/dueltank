import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {APP_CONFIG} from "./app-config.service";
import {UserProfile} from "../models/userprofile";
import {HttpClient} from "@angular/common/http";

@Injectable()
export class AccountsService {
  constructor(private http: HttpClient){}

  public Profile() : Observable<UserProfile> {
    // Make the API call using the new parameters.
    return this.http.get<UserProfile>(APP_CONFIG.apiEndpoint + "/api/accounts/profile");
  }
}
