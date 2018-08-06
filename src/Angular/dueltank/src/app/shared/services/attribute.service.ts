import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {Category} from "../models/category.model";
import {Attribute} from "../models/attribute.model";

@Injectable()
export class AttributeService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public allAttributes(): Observable<Attribute[]>{
    return this.http.get<Attribute[]>(this.configuration.apiEndpoint + "/api/attributes");
  }
}
