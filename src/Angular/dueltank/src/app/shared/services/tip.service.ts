import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {TipSection} from "../models/tipSection";


@Injectable()
export class TipService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public getTipsByCardId(cardId: Number): Observable<TipSection[]>{
    return this.http.get<TipSection[]>(this.configuration.apiEndpoint + "/api/cards/" + cardId + "/tips");
  }
}
