import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "./app-config.service";
import {Observable} from "rxjs";
import {Category} from "../models/category.model";

@Injectable()
export class CategoryService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public allCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.configuration.apiEndpoint + "/api/categories");
  }
}
