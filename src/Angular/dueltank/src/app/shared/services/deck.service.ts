import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs/Observable";
import {Deck} from "../models/deck";
import {AppConfigService} from "./app-config.service";
import * as FileSaver from "file-saver";
import * as Enumerable from "linq-es2015";
import {asEnumerable} from "linq-es2015";


@Injectable()
export class DeckService {
  constructor(private http: HttpClient, private configuration: AppConfigService){}

  public getDeckById(deckId: number) : Observable<Deck> {
    return this.http.get<Deck>(this.configuration.apiEndpoint + "/api/decks/" + deckId)
  }

  public downloadYdk(deck: Deck): void {
    var text = [];

    text.push("#created by ...");

    // main cards
    text.push("#main");
    deck.mainDeck.forEach(card => text.push(card.cardNumber))

    // extra cards
    text.push("#extra");
    deck.extraDeck.forEach(card => text.push(card.cardNumber))

    // side cards
    text.push("!side");
    deck.sideDeck.forEach(card => text.push(card.cardNumber))

    var data = new Blob([text.join("\n")], { type: 'text/plain;charset=utf-8' });
    FileSaver.saveAs(data, deck.sanitizedName + '.ydk');
  }

  public deckToText(deck: Deck) : string {
    var text = [];

    text.push(deck.name + "\nBy " + deck.username);
    text.push(Array(deck.name.length).join("-"));

    var numberOfCopiesByMainCards =  Enumerable
                                      .from(deck.mainDeck)
                                      .Distinct(card => card.name)
                                      .Select(card => {
                                        return {
                                          name: card.name,
                                          count: asEnumerable(deck.mainDeck)
                                            .Where(c => c.name == card.name)
                                            .Count()
                                        }
                                      })
                                      .ToArray();

    var numberOfCopiesByExtraCards =  Enumerable
                                      .from(deck.extraDeck)
                                      .Distinct(card => card.name)
                                      .Select(card => {
                                        return {
                                          name: card.name,
                                          count: asEnumerable(deck.extraDeck)
                                            .Where(c => c.name == card.name)
                                            .Count()
                                        }
                                      })
                                      .ToArray();

    var numberOfCopiesBySideCards =  Enumerable
                                      .from(deck.sideDeck)
                                      .Distinct(card => card.name)
                                      .Select(card => {
                                        return {
                                          name: card.name,
                                          count: asEnumerable(deck.sideDeck)
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
}

