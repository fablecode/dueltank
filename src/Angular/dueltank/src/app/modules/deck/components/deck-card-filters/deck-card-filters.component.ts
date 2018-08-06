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

  public formats: Format[];
  public categories: Category[];
  public subCategories: SubCategory[];
  public selectedSubCategories: SubCategory[];
  public attributes: Attribute[];

  constructor
  (
    private fb: FormBuilder,
    private formatService: FormatService,
    private categoryService: CategoryService,
    private subCategoryService: SubCategoryService,
    private attributeService: AttributeService
  ) { }

  ngOnInit(): void {
    this.banlistControl = new FormControl(null, Validators.required);
    this.categoryControl = new FormControl(null);
    this.subCategoryControl = new FormControl({value: '', disabled: true});
    this.attributeControl = new FormControl({value: '', disabled: true});

    this.cardFilterForm = this.fb.group({
      banlist: this.banlistControl,
      category: this.categoryControl,
      subCategory: this.subCategoryControl,
      attribute: this.attributeControl
    });

    this.getDeckCardSearchFilters()
        .subscribe(([formats, categories, subCategories, attributes]) => {
          this.formats = formats;
          this.categories = categories;
          this.subCategories = subCategories;
          this.selectedSubCategories = subCategories;
          this.attributes = attributes;

          this.onStateChanges();
        });
  }

  private getDeckCardSearchFilters() : Observable<any> {
    return forkJoin(
      this.formatService.allFormats(),
      this.categoryService.allCategories(),
      this.subCategoryService.allSubCategories(),
      this.attributeService.allAttributes()
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
        } else {
          let selectedCategory: Category = this.cardFilterForm.controls.category.value;

          this.selectedSubCategories = this.subCategories.filter(subCategory => subCategory.categoryId == selectedCategory.id);
          this.cardFilterForm.controls.subCategory.enable();

          if(selectedCategory.name === "Monster") {
            this.cardFilterForm.controls.attribute.enable();
          }
          else {
            this.cardFilterForm.controls.attribute.reset();
            this.cardFilterForm.controls.attribute.disable();
          }
        }
      });
  }
}
