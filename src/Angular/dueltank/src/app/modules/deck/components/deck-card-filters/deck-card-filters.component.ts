import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {FormatService} from "../../../../shared/services/format.service";
import {combineLatest, forkJoin, Observable} from "rxjs";
import {Format} from "../../../../shared/models/format";

@Component({
  templateUrl: "./deck-card-filters.component.html",
  selector: "deckCardFilters"
})
export class DeckCardFiltersComponent implements OnInit {
  public cardFilterForm : FormGroup;
  public banlistControl: FormControl;
  public categoryControl: FormControl;

  public formats: Format[];

  constructor(private fb: FormBuilder, private formatService: FormatService) { }

  ngOnInit(): void {
    this.banlistControl = new FormControl(null, Validators.required);
    this.categoryControl = new FormControl(null);

    this.cardFilterForm = this.fb.group({
      banlist: this.banlistControl,
      category: this.categoryControl
    });

    this.getDeckCardSearchFilters()
        .subscribe(([formats]) => {
          this.formats = formats;
        });
  }

  private getDeckCardSearchFilters() : Observable<any> {
    return forkJoin(
      this.formatService.allFormats()
    );
  }
}
