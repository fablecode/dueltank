import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AppConfigService} from "../../../shared/services/app-config.service";
import {DeckSearchResult} from "../../../shared/models/deck-search-result";
import {Observable} from "rxjs";

@Injectable()
export class UserDecksService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public authenticatedUserDecks(searchTerm: string, pageSize: number, pageIndex: number) : Observable<DeckSearchResult> {
    let httpParams = new HttpParams();

    if(searchTerm) {
      httpParams = httpParams.append("searchTerm", searchTerm)
    }

    httpParams = httpParams
      .set("pageSize", String(pageSize))
      .set("pageIndex", String(pageIndex));

    return this.http.get<DeckSearchResult>(this.configuration.apiEndpoint + "/api/userdecks", {params: httpParams})
  }

  public decksByUsername(username: string, searchTerm: string, pageSize: number, pageIndex: number) : Observable<DeckSearchResult> {
    let httpParams = new HttpParams();

    if(searchTerm) {
      httpParams = httpParams.append("searchTerm", searchTerm)
    }

    httpParams = httpParams
      .set("pageSize", String(pageSize))
      .set("pageIndex", String(pageIndex));

    return this.http.get<DeckSearchResult>(this.configuration.apiEndpoint + "/api/users/" + username + "/decks", {params: httpParams})
  }

  public deleteDeckById(id: number) : Observable<DeleteDeckResult> {
    let httpParams = new HttpParams();

    httpParams = httpParams
      .set("id", String(id));

    return this.http.delete<DeleteDeckResult>(this.configuration.apiEndpoint + "/api/userdecks", {params: httpParams})
  }
}

export class DeleteDeckResult {
  public id: number;
}
