import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {DeckCardSearchModel} from "../models/forms/deck-card-search.model";
import {Observable} from "rxjs";
import {CardSearchResult} from "../models/cardSearchResult.model";
import {Card} from "../models/card";

@Injectable()
export class CardSearchService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public search(searchCriteria: DeckCardSearchModel): Observable<CardSearchResult> {

    let httpParams = new HttpParams();

    if(searchCriteria.limit) {
      httpParams = httpParams.append("banlistId", searchCriteria.banlist.latestBanlist.id.toString())
      httpParams = httpParams.append("limitId", searchCriteria.limit.id.toString())
    }

    if(searchCriteria.category) {
      httpParams = httpParams.append("categoryId", searchCriteria.category.id.toString())
    }

    if(searchCriteria.subCategory) {
      httpParams = httpParams.append("subcategoryId", searchCriteria.subCategory.id.toString())
    }

    if(searchCriteria.attribute) {
      httpParams = httpParams.append("attributeId", searchCriteria.attribute.id.toString())
    }

    if(searchCriteria.type) {
      httpParams = httpParams.append("typeId", searchCriteria.type.id.toString())
    }

    if(searchCriteria.lvlrank) {
      httpParams = httpParams.append("lvlRank", searchCriteria.lvlrank.toString())
    }

    if(searchCriteria.atk != null) {
      httpParams = httpParams.append("atk", searchCriteria.atk.toString())
    }

    if(searchCriteria.def != null) {
      httpParams = httpParams.append("def", searchCriteria.def.toString())
    }

    if(searchCriteria.searchText != null) {
      httpParams = httpParams.append("searchTerm", searchCriteria.searchText)
    }

      httpParams = httpParams.append("pageSize", searchCriteria.pageSize.toString());
      httpParams = httpParams.append("pageIndex", searchCriteria.pageIndex.toString());

    return this.http.get<CardSearchResult>(this.configuration.apiEndpoint + "/api/searches/cards", {params: httpParams})
  }

  public getCardByName(name: string) : Observable<Card> {
    let httpParams = new HttpParams();

    httpParams.append("name", name);

    return this.http.get<Card>(this.configuration.apiEndpoint + "/api/cards", {params: httpParams})
  }
}
