import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {Format} from "../models/format";

@Injectable()
export class FormatService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public allFormats(): Observable<Format[]>{
    return this.http.get<Format[]>(this.configuration.apiEndpoint + "/api/formats");
  }
}


