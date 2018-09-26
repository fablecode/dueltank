import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {DeckCardSearchModel} from "../../../shared/models/forms/deck-card-search.model";
import {Format} from "../../../shared/models/format";

@Injectable()
export class DeckCardFilterService {
  public formats: Format[];

  // Observable sources
  private cardFiltersLoadedSource = new Subject<boolean>();
  private cardFiltersFormSubmittedSource = new Subject<DeckCardSearchModel>();
  private banlistChangedSource = new Subject<Format>();

  // Observable streams
  public cardFiltersLoaded$ = this.cardFiltersLoadedSource.asObservable();
  public cardFiltersFormSubmittedSource$ = this.cardFiltersFormSubmittedSource.asObservable();
  public banlistChangedSource$ = this.banlistChangedSource.asObservable();

  // Service message commands
  public cardFiltersLoaded(isLoaded: boolean) : void {
    this.cardFiltersLoadedSource.next(isLoaded);
  }

  public cardFiltersFormSubmitted(formModel : DeckCardSearchModel)  : void {
    this.cardFiltersFormSubmittedSource.next(formModel);
  }

  public banlistChanged(format: Format) {
    this.banlistChangedSource.next(format);
  }
}
