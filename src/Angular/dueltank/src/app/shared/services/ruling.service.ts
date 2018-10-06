import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {RulingSection} from "../models/rulingSection";

@Injectable()
export class RulingService {
  constructor(private http: HttpClient, private configuration: AppConfigService) {
  }

  public getRulingsByCardId(cardId: Number): Observable<RulingSection[]> {
    return this.http.get<RulingSection[]>(this.configuration.apiEndpoint + "/api/cards/" + cardId + "/rulings");
  }
}
