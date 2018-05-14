import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {UserProfile} from "../models/userprofile";
import {TokenService} from "./token.service";
import {BehaviorSubject} from "rxjs/BehaviorSubject";
import {AccountsService} from "./accounts.service";
import {UserProfileService} from "./userprofile.service";
import {ActivatedRoute, Router} from "@angular/router";
import {RegisterUser} from "../models/authentication/registeruser.model";
import {AuthenticatedUser} from "../models/authentication/authenticateduser.model";
import {LoginUser} from "../models/authentication/loginuser.model";

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

  public isLoginSubject = new BehaviorSubject<boolean>(this.hasToken());

  public externalLoginCallback(token: string): Observable<UserProfile> {
    this.tokenService.setAccessToken(token);
    return this.accountService.profile()
              .map(data => {
                  this.userProfileService.setUserProfile(data);
                  this.isLoginSubject.next(true);
                  return data;
              });
  }

  public register(newUser: RegisterUser, returnUrl: string) : Observable<UserProfile> {
    return this.accountService.register(newUser, (returnUrl || window.location.origin))
      .map(data  => {
        this.tokenService.setAccessToken(data.token);
        this.userProfileService.setUserProfile(data.user);
        this.isLoginSubject.next(true);
        return data.user
      })
  }

  public login(credentials: LoginUser, returnUrl: string) : Observable<UserProfile> {
    return this.accountService.login(credentials, (returnUrl || window.location.origin))
      .map(data  => {
        this.tokenService.setAccessToken(data.token);
        this.userProfileService.setUserProfile(data.user);
        this.isLoginSubject.next(true);
        return data.user
      })
  }

  public isLoggedIn() : Observable<boolean> {
    return this.isLoginSubject.asObservable();
  }

  /**
   * if we have token the user is loggedIn
   * @returns {boolean}
   */
  private hasToken() : boolean {
    return this.tokenService.hasToken();
  }

  public logOut() {
    this.tokenService.removeToken();
    this.userProfileService.removeUserProfile();
    this.isLoginSubject.next(false);
    this.router.navigate([""])
  }
}
