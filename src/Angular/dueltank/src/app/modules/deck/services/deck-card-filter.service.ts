import {Injectable} from "@angular/core";
import {Subject} from "rxjs";

@Injectable()
export class DeckCardFilterService {

  // Observable sources
  private cardFiltersLoadedSource = new Subject<boolean>();

  // Observable streams
  public cardFiltersLoaded$ = this.cardFiltersLoadedSource.asObservable();


  // Service message commands
  public cardFiltersLoaded(isLoaded: boolean) : void {
    this.cardFiltersLoadedSource.next(isLoaded);
  }
}
