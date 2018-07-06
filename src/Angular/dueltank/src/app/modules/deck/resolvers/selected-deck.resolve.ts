import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Deck} from "../../../shared/models/deck";
import {Observable} from "rxjs/Observable";
import {DeckService} from "../../../shared/services/deck.service";

@Injectable()
export class SelectedDeckResolve implements Resolve<Deck>{
  constructor(private deckService: DeckService ) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Deck> {
    var deckId = route.paramMap.get("id");

    return this.deckService.getDeckById(Number(deckId));
  }

}
