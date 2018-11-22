import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {Deck} from "../models/deck";
import {AppConfigService} from "./app-config.service";
import * as FileSaver from "file-saver";
import {Card} from "../models/card";
import {List} from "linqts";
import {DeckSearchResult} from "../models/deck-search-result";
import {DeckEditorInfo} from "../models/deck-editor-info";
import {AddDeckRequest} from "../models/add-deck-request";
import {UpdateDeckRequest} from "../models/update-deck-request";

@Injectable()
export class DeckService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public getDeckById(deckId: number) : Observable<Deck> {
    return this.http.get<Deck>(this.configuration.apiEndpoint + "/api/decks/" + deckId)
  }

  public search(searchTerm: string, pageSize: number, pageIndex: number) : Observable<DeckSearchResult>
  {
    let httpParams = new HttpParams();

    if(searchTerm) {
      httpParams = httpParams.append("searchTerm", searchTerm)
    }

    httpParams = httpParams
      .set("pageSize", String(pageSize))
      .set("pageIndex", String(pageIndex));

    return this.http.get<DeckSearchResult>(this.configuration.apiEndpoint + "/api/searches/decks", {params: httpParams});
  }

  public downloadYdk(deck: Deck): void {
    let text = [];

    text.push("#created by ...");

    // main cards
    text.push("#main");
    deck.mainDeck.forEach(card => text.push(card.cardNumber));

    // extra cards
    text.push("#extra");
    deck.extraDeck.forEach(card => text.push(card.cardNumber));

    // side cards
    text.push("!side");
    deck.sideDeck.forEach(card => text.push(card.cardNumber));

    let data = new Blob([text.join("\n")], { type: 'text/plain;charset=utf-8' });
    FileSaver.saveAs(data, deck.sanitizedName + '.ydk');
  }

  public deckToText(deck: Deck) : string {
    let text = [];

    let mainDeckCards = new List<Card>(deck.mainDeck);
    let extraDeckCards = new List<Card>(deck.extraDeck);
    let sideDeckCards = new List<Card>(deck.sideDeck);

    text.push(deck.name + "\nBy " + deck.username);
    text.push(Array(deck.name.length).join("-"));

    let numberOfCopiesByMainCards =  mainDeckCards
                                      .DistinctBy(card => card.name)
                                      .Select(card => {
                                        return {
                                          name: card.name,
                                          count: mainDeckCards
                                            .Where(c => c.name == card.name)
                                            .Count()
                                        }
                                      })
                                      .ToArray();

    let numberOfCopiesByExtraCards =  extraDeckCards
                                      .DistinctBy(card => card.name)
                                      .Select(card => {
                                        return {
                                          name: card.name,
                                          count: extraDeckCards
                                            .Where(c => c.name == card.name)
                                            .Count()
                                        }
                                      })
                                      .ToArray();

    let numberOfCopiesBySideCards =  sideDeckCards
                                      .DistinctBy(card => card.name)
                                      .Select(card => {
                                        return {
                                          name: card.name,
                                          count: sideDeckCards
                                            .Where(c => c.name == card.name)
                                            .Count()
                                        }
                                      })
                                      .ToArray();

    text.push("\n");

    text.push("Main Deck");
    text.push("---------");
    numberOfCopiesByMainCards.forEach(card => text.push(card.count + "x " + card.name));

    text.push("\n");

    text.push("Extra Deck");
    text.push("---------");
    numberOfCopiesByExtraCards.forEach(card => text.push(card.count + "x " + card.name));

    text.push("\n");

    text.push("Side Deck");
    text.push("---------");
    numberOfCopiesBySideCards.forEach(card => text.push(card.count + "x " + card.name));

    return text.join("\n");
  }

  public mostRecentDecks(pageSize: number) {
    let httpParams = new HttpParams();

    httpParams = httpParams
      .set("pageSize", String(pageSize));

    return this.http.get<DeckSearchResult>(this.configuration.apiEndpoint + "/api/decks/latest", {params: httpParams})
  }

  public addDeck(deckEditorInfo: DeckEditorInfo, newDeck: Deck) : Observable<AddDeckResult> {

    let requestData = new AddDeckRequest();

    requestData.name = deckEditorInfo.name;
    requestData.description = deckEditorInfo.description;
    requestData.videoUrl = deckEditorInfo.videoUrl;
    requestData.mainDeck = newDeck.mainDeck;
    requestData.extraDeck = newDeck.extraDeck;
    requestData.sideDeck = newDeck.sideDeck;

    return this.http.post<AddDeckResult>(this.configuration.apiEndpoint + "/api/decks/", requestData)
  }

  public updateDeck(deckEditorInfo: DeckEditorInfo, existingDeck: Deck) : Observable<AddDeckResult> {

    let requestData = new UpdateDeckRequest();

    requestData.id = existingDeck.id;
    requestData.name = deckEditorInfo.name;
    requestData.description = deckEditorInfo.description;
    requestData.videoUrl = deckEditorInfo.videoUrl;
    requestData.mainDeck = existingDeck.mainDeck;
    requestData.extraDeck = existingDeck.extraDeck;
    requestData.sideDeck = existingDeck.sideDeck;

    return this.http.post<AddDeckResult>(this.configuration.apiEndpoint + "/api/decks/", requestData)
  }
}

export class AddDeckResult {
}

