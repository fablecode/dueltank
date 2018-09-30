import {Component, OnDestroy, OnInit} from "@angular/core";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {FormatService} from "../../../../shared/services/format.service";
import {forkJoin, Observable, ReplaySubject} from "rxjs";
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
import {DeckCardSearchModel} from "../../../../shared/models/forms/deck-card-search.model";
import {DeckCardFilterService} from "../../services/deck-card-filter.service";
import {distinctUntilChanged} from "rxjs/operators";
import {Type} from "../../../../shared/models/type.model";

@Component({
  templateUrl: "./deck-card-filters.component.html",
  selector: "deckCardFilters"
})
export class DeckCardFiltersComponent implements OnInit, OnDestroy {
  // form
  public cardFilterForm : FormGroup;

  // form controls
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
  public types: Type[];
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
  ) {}

  ngOnInit(): void {
    this.banlistControl = new FormControl(null);
    this.categoryControl = new FormControl(null);
    this.subCategoryControl = new FormControl({value: null, disabled: true});
    this.attributeControl = new FormControl({value: null, disabled: true});
    this.typeControl = new FormControl({value: null, disabled: true});
    this.lvlrankControl = new FormControl({value: null, disabled: true}, [Validators.pattern("^[0-9]*$")]);
    this.limitControl = new FormControl(null);
    this.atkControl = new FormControl({value: null, disabled: true}, [Validators.pattern("^[0-9]*$")]);
    this.defControl = new FormControl({value: null, disabled: true}, [Validators.pattern("^[0-9]*$")]);
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
    this.onStateChanges();
    this.getDeckCardSearchFilters()
        .subscribe(([formats, categories, subCategories, attributes, types, limits]) => {
          this.formats = formats;
          this.categories = categories;
          this.subCategories = subCategories;
          this.selectedSubCategories = subCategories;
          this.attributes = attributes;
          this.types = types;
          this.limits = limits;
          this.InitializeDropdownDefaultValues();


          this.deckCardFilterService.banlistLoaded(formats[0]);
          this.deckCardFilterService.cardFiltersLoaded(true);
          this.onSubmitSearch();
        });
  }

  private InitializeDropdownDefaultValues() {
    this.banlistControl.setValue(this.formats[0]);
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
    // Banlist dropdown changes
    this.cardFilterForm
      .controls
      .banlist
      .valueChanges
      .subscribe((format: Format) => {
        this.deckCardFilterService.banlistChanged(format);
      });

    // Category dropdown changes
    this.cardFilterForm
      .controls
      .category
      .valueChanges
      .subscribe((selectedCategory: Category) => {
        if(selectedCategory == null) {
          this.resetCategory();
        } else {
          let selectedCategory: Category = this.cardFilterForm.controls.category.value;

          this.selectedSubCategories = this.subCategories.filter(subCategory => subCategory.categoryId == selectedCategory.id);
          this.cardFilterForm.controls.subCategory.enable({emitEvent: false});

          if(selectedCategory.name === "Monster") {
            this.cardFilterForm.controls.attribute.enable({emitEvent: false});
            this.cardFilterForm.controls.type.enable({emitEvent: false});
            this.cardFilterForm.controls.lvlrank.enable({emitEvent: false});
            this.cardFilterForm.controls.atk.enable({emitEvent: false});
            this.cardFilterForm.controls.def.enable({emitEvent: false});
          }
          else {
            // Attribute
            this.cardFilterForm.controls.attribute.reset(null, {emitEvent: false});
            this.cardFilterForm.controls.attribute.disable({emitEvent: false});

            // Type
            this.cardFilterForm.controls.type.reset(null, {emitEvent: false});
            this.cardFilterForm.controls.type.disable({emitEvent: false});

            // Lvlrank
            this.cardFilterForm.controls.lvlrank.reset(null, {emitEvent: false});
            this.cardFilterForm.controls.lvlrank.disable({emitEvent: false});

            // Atk
            this.cardFilterForm.controls.atk.reset(null, {emitEvent: false});
            this.cardFilterForm.controls.atk.disable({emitEvent: false});

            // Def
            this.cardFilterForm.controls.def.reset(null, {emitEvent: false});
            this.cardFilterForm.controls.def.disable({emitEvent: false});
          }
        }

        this.onSubmitSearch();
      });

      // SubCategory
    this.subCategoryControl
        .valueChanges
        .subscribe((subCategory: SubCategory) => {
          this.onSubmitSearch();
        });

      // Attribute
    this.attributeControl
        .valueChanges
        .subscribe((attribute: Attribute) => {
          this.onSubmitSearch();
        });

      // Type
    this.typeControl
        .valueChanges
        .subscribe((type: Type) => {
          this.onSubmitSearch();
        });

    // SearchText
    this.searchControl
      .valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .subscribe((term: string) => {
        console.log(new DeckCardSearchModel(this.cardFilterForm.getRawValue()))
        this.onSubmitSearch();
      });

    // Limit
    this.limitControl
      .valueChanges
      .subscribe((limit: Limit) => {
        this.onSubmitSearch();
      });
  }

  private resetCategory() {
    // SubCategory
    this.selectedSubCategories = this.subCategories;
    this.cardFilterForm.controls.subCategory.reset(null, {emitEvent: false});
    this.cardFilterForm.controls.subCategory.disable({emitEvent: false});

    // Attribute
    this.cardFilterForm.controls.attribute.reset(null, {emitEvent: false});
    this.cardFilterForm.controls.attribute.disable({emitEvent: false});

    // Type
    this.cardFilterForm.controls.type.reset(null, {emitEvent: false});
    this.cardFilterForm.controls.type.disable({emitEvent: false});

    // Lvl or Rank
    this.cardFilterForm.controls.lvlrank.reset(null, {emitEvent: false});
    this.cardFilterForm.controls.lvlrank.disable({emitEvent: false});

    // Atk
    this.cardFilterForm.controls.atk.reset(null, {emitEvent: false});
    this.cardFilterForm.controls.atk.disable({emitEvent: false});

    // Def
    this.cardFilterForm.controls.def.reset(null, {emitEvent: false});
    this.cardFilterForm.controls.def.disable({emitEvent: false});
  }

  public onSubmitSearch() : void {
    if(this.cardFilterForm.valid) {
      let searchModel: DeckCardSearchModel = new DeckCardSearchModel(this.cardFilterForm.getRawValue());
      this.deckCardFilterService.cardFiltersFormSubmitted(searchModel);
    }
    else {
      console.log("Form not valid!")
    }
  }

  public onReset() : void {
    this.banlistControl.setValue(this.formats[0]);
    this.resetCategory();
    this.categoryControl.reset(null,{emitEvent: false})
    this.searchControl.reset(null,{emitEvent: false})
    this.limitControl.reset(null);
  }

  ngOnDestroy(): void {
  }
}
