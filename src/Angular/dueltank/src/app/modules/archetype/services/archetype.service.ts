import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {HttpClient, HttpParams} from "@angular/common/http";
import {AppConfigService} from "../../../shared/services/app-config.service";
import {ArchetypeSearchResult} from "../../../shared/models/archetype-search-result";
import {Archetype} from "../../../shared/models/archetype";

@Injectable()
export class ArchetypeService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}
  public search(searchTerm: string, pageSize: number, pageIndex: number) : Observable<ArchetypeSearchResult>
  {
    let httpParams = new HttpParams();

    if(searchTerm) {
      httpParams = httpParams.append("searchTerm", searchTerm)
    }

    httpParams = httpParams
      .set("pageSize", String(pageSize))
      .set("pageIndex", String(pageIndex));

    return this.http.get<ArchetypeSearchResult>(this.configuration.apiEndpoint + "/api/searches/archetypes", {params: httpParams})
  }

  public archetypeById(archetypeId: number) : Observable<Archetype> {
    return this.http.get<Archetype>(this.configuration.apiEndpoint + "/api/archetypes/" + archetypeId);
  }

  public mostRecentArchetypes(pageSize: number) : Observable<ArchetypeSearchResult> {
    let httpParams = new HttpParams();

    httpParams = httpParams
      .set("pageSize", String(pageSize));

    return this.http.get<ArchetypeSearchResult>(this.configuration.apiEndpoint + "/api/archetypes/latest", {params: httpParams})
  }
}
