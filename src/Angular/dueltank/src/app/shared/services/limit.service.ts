import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {Type} from "../models/type.model";

@Injectable()
export class LimitService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public allLimits(): Observable<Type[]>{
    return this.http.get<Type[]>(this.configuration.apiEndpoint + "/api/limits");
  }
}
