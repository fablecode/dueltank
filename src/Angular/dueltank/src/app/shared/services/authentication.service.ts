import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
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
