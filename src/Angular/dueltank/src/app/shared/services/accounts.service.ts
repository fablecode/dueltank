import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import { AppConfigService} from "./app-config.service";
import {UserProfile} from "../models/userprofile";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AuthenticatedUser} from "../models/authentication/authenticateduser.model";
import {RegisterUser} from "../models/authentication/registeruser.model";

@Injectable()
export class AccountsService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public Profile() : Observable<UserProfile> {
    return this.http.get<UserProfile>(this.configuration.apiEndpoint + "/api/accounts/profile");
  }

  public Register(user : RegisterUser, returnUrl: string) : Observable<AuthenticatedUser> {
    let httpParams = new HttpParams()
                      .set("returnUrl", returnUrl);
    return this.http.post<AuthenticatedUser>(this.configuration.apiEndpoint + "/api/accounts/register", user, { params: httpParams});
  }
}
