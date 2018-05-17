
import {throwError as observableThrowError} from 'rxjs';
import { Injectable } from "@angular/core";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {AppConfig} from "../models/app-config";
import {catchError, map} from "rxjs/operators";

 export let APP_CONFIG: AppConfig;

@Injectable()
export class AppConfigService {

  constructor(private http: HttpClient) {}

  public load() {
    return new Promise(((resolve, reject) => {
      this.http.get("/assets/config/config.json")
        .pipe(map(res => res),
          catchError((error: HttpErrorResponse) => {
              reject(true);
              return observableThrowError("Server error: " + error);
            }
          )
        )
        .subscribe((envResponse: any) => {
          let ac = new AppConfig();
          APP_CONFIG = Object.assign(ac, envResponse);
          resolve(true);
        });
    }));
  }

  get apiEndpoint() : string{
    return APP_CONFIG.apiEndpoint;
  }
}
