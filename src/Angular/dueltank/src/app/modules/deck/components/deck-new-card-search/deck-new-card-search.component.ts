import {Component, Input, OnInit} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {ToastrService} from "ngx-toastr";
import {DeckInfoService} from "../../services/deck-info.service";
import {DeckEditorInfo} from "../../../../shared/models/deck-editor-info";

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
  public deckDescription: FormControl;
  public deckVideoUrl: FormControl;

  // deck thumbnail
  private selectedThumbnailFile : File;

  constructor
  (
    private fb: FormBuilder,
    private seo: SearchEngineOptimizationService,
    private toastr: ToastrService,
    private deckInfoService: DeckInfoService
  ) {
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

  public onSubmit() : void {
    if(this.newDeckForm.valid) {
     let deckInfo : DeckEditorInfo = new DeckEditorInfo();

     deckInfo.thumbnail = this.selectedThumbnailFile;
     deckInfo.name = this.deckName.value;
     deckInfo.description = this.deckDescription.value;
     deckInfo.videoUrl = this.deckVideoUrl.value;

     console.log(deckInfo);
     //this.deckInfoService.saveDeck(deckInfo);
    }
  }

  private createForm() {
    this.thumbnailFileSelected = new FormControl("");
    this.deckName = new FormControl("", [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(255)
    ]);
    this.deckDescription = new FormControl("", [
      Validators.maxLength(2083)
    ]);
    this.deckVideoUrl = new FormControl("", [
      Validators.maxLength(2083)
    ]);

    this.newDeckForm = this.fb.group({
      thumbnailFile: this.thumbnailFileSelected,
      deckName: this.deckName,
      deckDescription: this.deckDescription,
      deckVideoUrl: this.deckVideoUrl
    });
  }
}
