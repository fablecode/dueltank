
import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {UserProfile} from "../models/userprofile";
import {TokenService} from "./token.service";
import {AccountsService} from "./accounts.service";
import {UserProfileService} from "./userprofile.service";
import {Router} from "@angular/router";
import {RegisterUser} from "../models/authentication/registeruser.model";
import {LoginUser} from "../models/authentication/loginuser.model";
import {BehaviorSubject} from "rxjs/internal/BehaviorSubject";
import {Observable} from "rxjs/internal/Observable";
import {map} from "rxjs/operators";
import {UserForgotPassword} from "../models/authentication/userforgotpassword.model";
import {ResetUserPassword} from "../models/reset-user-password";
import {Globals} from "../../globals";

@Injectable()
export class AuthenticationService {
  public redirectUrl: string;
  constructor
  (
    private http: HttpClient,
    private router: Router,

    private tokenService: TokenService,
    private accountService: AccountsService,
    private userProfileService: UserProfileService,
    private globals : Globals
  )
  {}

  public isLoginSubject = new BehaviorSubject<boolean>(this.hasToken());

  public externalLoginCallback(token: string): Observable<UserProfile> {
    this.tokenService.setAccessToken(token);
    return this.accountService.profile().pipe(
              map(data => {
                  this.userProfileService.setUserProfile(data);
                  this.isLoginSubject.next(true);
                  return data;
              }));
  }

  public register(newUser: RegisterUser, returnUrl: string) : Observable<UserProfile> {
    return this.accountService.register(newUser, returnUrl).pipe(
      map(data  => {
        this.tokenService.setAccessToken(data.token);
        this.userProfileService.setUserProfile(data.user);
        this.isLoginSubject.next(true);
        return data.user
      }))
  }

  public login(credentials: LoginUser, returnUrl: string) : Observable<UserProfile> {
    return this.accountService.login(credentials, returnUrl).pipe(
      map(data  => {
        this.tokenService.setAccessToken(data.token);
        this.userProfileService.setUserProfile(data.user);
        this.isLoginSubject.next(true);
        return data.user
      }))
  }

  public isLoggedIn() : BehaviorSubject<boolean> {
    return this.isLoginSubject;
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

  public forgotPassword(userForgotPassword: UserForgotPassword) {
    return this.accountService.forgotPassword(userForgotPassword);
  }

  public resetPassword(resetUserPassword: ResetUserPassword) {
    return this.accountService.resetPassword(resetUserPassword);
  }

  public checkUsernameNotTaken(username: string) {
    return this.accountService.checkUsernameNotTaken(username);
  }
}
