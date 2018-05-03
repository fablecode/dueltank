import {Injectable} from "@angular/core";
import {HttpClient, HttpParams, HttpResponse} from "@angular/common/http";
import {APP_CONFIG} from "./app-config.service";
import {Externallogin} from "../models/externallogin";
import {Observable} from "rxjs/Observable";
import {UserProfile} from "../models/userprofile";
import {TokenService} from "./token.service";
import {BehaviorSubject} from "rxjs/BehaviorSubject";
import {AccountsService} from "./accounts.service";
import {UserProfileService} from "./userprofile.service";
import {Router} from "@angular/router";

@Injectable()
export class AuthenticationService {
  constructor
  (
    private http: HttpClient,
    private router: Router,
    private tokenService: TokenService,
    private accountService: AccountsService,
    private userProfileService: UserProfileService
  )
  {}

  public isLoginSubject = new BehaviorSubject<boolean>(this.tokenService.hasToken());

  public externalLogin(externallogin : Externallogin) : Observable<HttpResponse<any>> {
    // Setup log namespace query parameter
    let params = new HttpParams();

    // Begin assigning parameters
    params = params.append("provider", externallogin.provider);
    params = params.append("returnUrl", externallogin.returnUrl);
    params = params.append("observe", "response");

    // Make the API call using the new parameters.
    return this.http.get<HttpResponse<any>>
      (
        APP_CONFIG.apiEndpoint + "/api/accounts/externalLogin",
        {params: params}
      );
  }

  public externalLoginCallback(token: string): Observable<UserProfile> {
    this.tokenService.setAccessToken(token);
    return this.accountService.Profile()
              .map(data => {
                  this.userProfileService.setUserProfile(data);
                  return data;
              });
  }

  public isLoggedIn() : Observable<boolean> {
    return this.isLoginSubject.asObservable();
  }


}
