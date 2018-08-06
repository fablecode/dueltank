import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {SubCategory} from "../models/subcategory.model";

@Injectable()
export class SubCategoryService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public allSubCategories(): Observable<SubCategory[]>{
    return this.http.get<SubCategory[]>(this.configuration.apiEndpoint + "/api/subcategories");
  }
}
