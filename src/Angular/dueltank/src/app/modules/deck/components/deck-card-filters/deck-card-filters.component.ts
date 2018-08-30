import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {FormatService} from "../../../../shared/services/format.service";
import {forkJoin, Observable} from "rxjs";
import {Format} from "../../../../shared/models/format";
import {CategoryService} from "../../../../shared/services/category.service";
import {Category} from "../../../../shared/models/category.model";
import {SubCategoryService} from "../../../../shared/services/subcategory.service";
import {SubCategory} from "../../../../shared/models/subcategory.model";
import {AttributeService} from "../../../../shared/services/attribute.service";
import {Attribute} from "../../../../shared/models/attribute.model";
import {TypeService} from "../../../../shared/services/type.service";
import {LimitService} from "../../../../shared/services/limit.service";
import {Limit} from "../../../../shared/models/limit.model";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {DeckCardSearchModel} from "../../../../shared/models/forms/deck-card-search.model";

@Component({
  templateUrl: "./deck-card-filters.component.html",
  selector: "deckCardFilters"
})
export class DeckCardFiltersComponent implements OnInit {
  public cardFilterForm : FormGroup;
  public banlistControl: FormControl;
  public categoryControl: FormControl;
  public subCategoryControl: FormControl;
  public attributeControl: FormControl;
  public typeControl: FormControl;
  public lvlrankControl: FormControl;
  public limitControl: FormControl;
  public atkControl: FormControl;
  public defControl: FormControl;
  public searchControl: FormControl;

  public formats: Format[];
  public categories: Category[];
  public subCategories: SubCategory[];
  public selectedSubCategories: SubCategory[];
  public attributes: Attribute[];
  public types: Attribute[];
  public limits: Limit[];

  constructor
  (
    private fb: FormBuilder,
    private formatService: FormatService,
    private categoryService: CategoryService,
    private subCategoryService: SubCategoryService,
    private attributeService: AttributeService,
    private typeService: TypeService,
    private limitService: LimitService,
    private deckCardFilterService : DeckCardFilterService
  ) { }

  ngOnInit(): void {
    this.banlistControl = new FormControl(null);
    this.categoryControl = new FormControl(null);
    this.subCategoryControl = new FormControl({value: '', disabled: true});
    this.attributeControl = new FormControl({value: '', disabled: true});
    this.typeControl = new FormControl({value: '', disabled: true});
    this.lvlrankControl = new FormControl({value: '', disabled: true});
    this.limitControl = new FormControl({value: ''});
    this.atkControl = new FormControl({value: '', disabled: true});
    this.defControl = new FormControl({value: '', disabled: true});
    this.searchControl = new FormControl('');

    this.cardFilterForm = this.fb.group({
      banlist: this.banlistControl,
      category: this.categoryControl,
      subCategory: this.subCategoryControl,
      attribute: this.attributeControl,
      type: this.typeControl,
      lvlrank: this.lvlrankControl,
      limit: this.limitControl,
      atk: this.atkControl,
      def: this.defControl,
      searchText: this.searchControl
    });

    this.getDeckCardSearchFilters()
        .subscribe(([formats, categories, subCategories, attributes, types, limits]) => {
          this.formats = formats;
          this.categories = categories;
          this.subCategories = subCategories;
          this.selectedSubCategories = subCategories;
          this.attributes = attributes;
          this.types = types;
          this.limits = limits;
          this.onStateChanges();
          this.deckCardFilterService.cardFiltersLoaded(true);
        });
  }

  private getDeckCardSearchFilters() : Observable<any> {
    return forkJoin(
      this.formatService.allFormats(),
      this.categoryService.allCategories(),
      this.subCategoryService.allSubCategories(),
      this.attributeService.allAttributes(),
      this.typeService.allTypes(),
      this.limitService.allLimits()
    );
  }

  private onStateChanges() {
    this.cardFilterForm
      .controls
      .category
      .valueChanges
      .subscribe((selectedCategory: Category) => {
        if(selectedCategory == null) {
          // SubCategory
          this.selectedSubCategories = this.subCategories;
          this.cardFilterForm.controls.subCategory.reset();
          this.cardFilterForm.controls.subCategory.disable();

          // Attribute
          this.cardFilterForm.controls.attribute.reset();
          this.cardFilterForm.controls.attribute.disable();

          // Type
          this.cardFilterForm.controls.type.reset();
          this.cardFilterForm.controls.type.disable();

          // Lvl or Rank
          this.cardFilterForm.controls.lvlrank.reset();
          this.cardFilterForm.controls.lvlrank.disable();

          // Atk
          this.cardFilterForm.controls.atk.reset();
          this.cardFilterForm.controls.atk.disable();

          // Def
          this.cardFilterForm.controls.def.reset();
          this.cardFilterForm.controls.def.disable();
        } else {
          let selectedCategory: Category = this.cardFilterForm.controls.category.value;

          this.selectedSubCategories = this.subCategories.filter(subCategory => subCategory.categoryId == selectedCategory.id);
          this.cardFilterForm.controls.subCategory.enable();

          if(selectedCategory.name === "Monster") {
            this.cardFilterForm.controls.attribute.enable();
            this.cardFilterForm.controls.type.enable();
            this.cardFilterForm.controls.lvlrank.enable();
            this.cardFilterForm.controls.atk.enable();
            this.cardFilterForm.controls.def.enable();
          }
          else {
            // Attribute
            this.cardFilterForm.controls.attribute.reset();
            this.cardFilterForm.controls.attribute.disable();

            // Type
            this.cardFilterForm.controls.type.reset();
            this.cardFilterForm.controls.type.disable();

            // Lvlrank
            this.cardFilterForm.controls.lvlrank.reset();
            this.cardFilterForm.controls.lvlrank.disable();

            // Atk
            this.cardFilterForm.controls.atk.reset();
            this.cardFilterForm.controls.atk.disable();

            // Def
            this.cardFilterForm.controls.def.reset();
            this.cardFilterForm.controls.def.disable();
          }
        }
      });
  }

  public onSubmit() {
    if(this.cardFilterForm.valid) {
      debugger;
      console.log(this.cardFilterForm.value);
      const result: DeckCardSearchModel = new DeckCardSearchModel(this.cardFilterForm.value);

      console.log(result);
    } else {
      console.log("Form not valid.");
    }
  }
}
