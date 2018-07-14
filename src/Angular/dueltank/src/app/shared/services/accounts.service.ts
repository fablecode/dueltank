import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import { AppConfigService} from "./app-config.service";
import {UserProfile} from "../models/userprofile";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AuthenticatedUser} from "../models/authentication/authenticateduser.model";
import {RegisterUser} from "../models/authentication/registeruser.model";
import {LoginUser} from "../models/authentication/loginuser.model";
import {UserForgotPassword} from "../models/authentication/userforgotpassword.model";
import {ResetUserPassword} from "../models/reset-user-password";
import {ExternalLoginConfirmation} from "../models/authentication/externalloginconfirmation.model";

@Injectable()
export class AccountsService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public profile() : Observable<UserProfile> {
    return this.http.get<UserProfile>(this.configuration.apiEndpoint + "/api/accounts/profile");
  }

  public register(user : RegisterUser, returnUrl: string) : Observable<AuthenticatedUser> {
    let httpParams = new HttpParams()
      .set("returnUrl", returnUrl);
    return this.http.post<AuthenticatedUser>(this.configuration.apiEndpoint + "/api/accounts/register", user, { params: httpParams});
  }

  public login(existingUser: LoginUser, returnUrl: string) : Observable<AuthenticatedUser> {
    let httpParams = new HttpParams()
      .set("returnUrl", returnUrl);
    return this.http.post<AuthenticatedUser>(this.configuration.apiEndpoint + "/api/accounts/login", existingUser, { params: httpParams});
}

  public forgotPassword(userForgotPassword: UserForgotPassword) : Observable<any>{
    return this.http.post(this.configuration.apiEndpoint + "/api/accounts/forgotpassword", userForgotPassword);
  }

  public resetPassword(resetUserPassword: ResetUserPassword) {
    let httpParams = new HttpParams()
      .set("code", resetUserPassword.code);
    return this.http.post(this.configuration.apiEndpoint + "/api/accounts/resetpassword", resetUserPassword, { params: httpParams});
  }

  public checkUsernameNotTaken(username: string) {
    let httpParams = new HttpParams()
      .set("username", username);
    return this.http.get(this.configuration.apiEndpoint + "/api/accounts/verifyusername", { params: httpParams})
  }

  public externalLoginConfirmation(externalLoginConfirmation: ExternalLoginConfirmation) {
    return this.http.post<AuthenticatedUser>(this.configuration.apiEndpoint + "/api/accounts/externalloginconfirmation", externalLoginConfirmation);
  }
}
