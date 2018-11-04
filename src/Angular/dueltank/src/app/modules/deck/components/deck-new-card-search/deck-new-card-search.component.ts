import {Component, Input, OnInit} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: "deckNewCardSearch",
  templateUrl: "./deck-new-card-search.component.html"
})
export class DeckNewCardSearchComponent implements OnInit {
  @Input() deck: Deck;

  // form group
  public newDeckForm: FormGroup;

  // form controls
  private thumbnailFileSelected: FormControl;
  public deckName: FormControl;

  // deck thumbnail
  private selectedThumbnailFile : File;

  constructor(private fb: FormBuilder, private seo: SearchEngineOptimizationService, private toastr: ToastrService) {
    this.seo.title("DuelTank - Create new yugioh deck");
    this.seo.description("Use the dueltank deck editor to create a new yugioh deck");
    this.seo.keywords("DuelTank, deck, new, create");
    this.seo.robots("index,follow");
  }

  ngOnInit(): void {
    this.createForm();
  }

  public deckThumbnailSelected(files: FileList) {
    if(files && files.length > 0 && files.length < 2) {
      if(files[0].size < 2000000) {
        this.selectedThumbnailFile = files[0];
        this.thumbnailFileSelected.setValue(files.item(0).name);
      } else {
        this.toastr.warning("Image file is too big! Only 2MB or less is allowed.");
      }

    } else {
      this.toastr.warning("Only 1 file allowed");
    }
  }

  public log (object: any ) {
    console.log(object);
  }
  public onSubmit() : void {
    console.log("New deck form was submitted.")
  }

  private createForm() {
    this.thumbnailFileSelected = new FormControl("");
    this.deckName = new FormControl("", [
      Validators.required,
      Validators.minLength(4),
      Validators.max(255)
    ]);

    this.newDeckForm = this.fb.group({
      thumbnailFile: this.thumbnailFileSelected,
      deckName: this.deckName
    });
  }
}
