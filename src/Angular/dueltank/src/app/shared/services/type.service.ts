import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {Category} from "../models/category.model";
import {Attribute} from "../models/attribute.model";
import {Type} from "../models/type.model";

@Injectable()
export class TypeService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public allAttributes(): Observable<Type[]>{
    return this.http.get<Type[]>(this.configuration.apiEndpoint + "/api/types");
  }
}
