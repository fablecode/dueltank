import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {Deck} from "../models/deck";
import {AppConfigService} from "./app-config.service";


@Injectable()
export class DeckService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public getDeckById(deckId: number) : Observable<Deck> {
    return this.http.get<Deck>(this.configuration.apiEndpoint + "/api/decks/" + deckId)
  }
}

