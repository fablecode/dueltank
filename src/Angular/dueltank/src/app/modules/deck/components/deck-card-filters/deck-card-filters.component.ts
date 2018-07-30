import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup} from "@angular/forms";

@Component({
  templateUrl: "./deck-card-filters.component.html",
  selector: "deckCardFilters"
})
export class DeckCardFiltersComponent implements OnInit {
  public cardFilterForm : FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.cardFilterForm = this.fb.group({
      banlist: ['']
    });
  }
}
