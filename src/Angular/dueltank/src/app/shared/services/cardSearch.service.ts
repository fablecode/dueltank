import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {DeckCardSearchModel} from "../models/forms/deck-card-search.model";
import {Observable} from "rxjs";
import {CardSearchResult} from "../models/cardSearchResult.model";

@Injectable()
export class CardSearchService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public search(searchCriteria: DeckCardSearchModel): Observable<CardSearchResult> {
    let httpParams = new HttpParams({ fromObject: <any>searchCriteria });
    return this.http.get<CardSearchResult>(this.configuration.apiEndpoint + "/api/searches/cards", {params: httpParams})
  }
}
