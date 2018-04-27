import {Injectable} from "@angular/core";
import {HttpClient, HttpParams, HttpResponse} from "@angular/common/http";
import {APP_CONFIG} from "./app-config.service";
import {Externallogin} from "../models/externallogin";
import {Observable} from "rxjs/Observable";

@Injectable()
export class AuthenticationService {
  constructor(private http: HttpClient){}

  public externalLogin(externallogin : Externallogin) : Observable<HttpResponse<any>> {
    // Setup log namespace query parameter
    let params = new HttpParams();

    // Begin assigning parameters
    params.append("provider", externallogin.provider);
    params.append("returnUrl", externallogin.returnUrl);
    params.append("observe", "response");

    // Make the API call using the new parameters.
    return this.http.get<HttpResponse<any>>
      (
        APP_CONFIG.apiEndpoint + "/api/accounts/externalLogin",
        {params: params}
      );
  }
}
