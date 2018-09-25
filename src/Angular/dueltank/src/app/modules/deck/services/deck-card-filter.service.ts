import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {DeckCardSearchModel} from "../../../shared/models/forms/deck-card-search.model";

@Injectable()
export class DeckCardFilterService {

  // Observable sources
  private cardFiltersLoadedSource = new Subject<boolean>();
  private cardFiltersFormSubmittedSource = new Subject<DeckCardSearchModel>();

  // Observable streams
  public cardFiltersLoaded$ = this.cardFiltersLoadedSource.asObservable();
  public cardFiltersFormSubmittedSource$ = this.cardFiltersFormSubmittedSource.asObservable();


  // Service message commands
  public cardFiltersLoaded(isLoaded: boolean) : void {
    this.cardFiltersLoadedSource.next(isLoaded);
  }

  public cardFiltersFormSubmitted(formModel : DeckCardSearchModel)  : void {
    this.cardFiltersFormSubmittedSource.next(formModel);
  }
}
