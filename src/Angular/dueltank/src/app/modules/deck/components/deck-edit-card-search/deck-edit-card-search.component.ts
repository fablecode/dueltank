import {Component, Input} from "@angular/core";
import {Deck} from "../../../../shared/models/deck";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {ToastrService} from "ngx-toastr";
import {DeckInfoService} from "../../services/deck-info.service";
import {DeckEditorInfo} from "../../../../shared/models/deck-editor-info";

@Component({
  selector: "deckEditCardSearch",
  templateUrl: "./deck-edit-card-search.component.html"
})
export class DeckEditCardSearchComponent {
  @Input() deck: Deck = new Deck();

  // form group
  public editDeckForm: FormGroup;

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
    this.seo.description("Use the dueltank deck editor to edit an existing yugioh deck");
    this.seo.keywords("DuelTank, deck, existing, edit");
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
    if(this.editDeckForm.valid) {
      let deckInfo : DeckEditorInfo = new DeckEditorInfo();

      deckInfo.thumbnail = this.selectedThumbnailFile;
      deckInfo.name = this.deckName.value;
      deckInfo.description = this.deckDescription.value;
      deckInfo.videoUrl = this.deckVideoUrl.value;

      this.deckInfoService.saveDeck(deckInfo);
    }
  }

  private createForm() {
    this.thumbnailFileSelected = new FormControl("");
    this.deckName = new FormControl(this.deck.name, [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(255)
    ]);
    this.deckDescription = new FormControl(this.deck.description, [
      Validators.maxLength(2083)
    ]);
    this.deckVideoUrl = new FormControl(this.deck.videoId, [
      Validators.maxLength(2083)
    ]);

    this.editDeckForm = this.fb.group({
      thumbnailFile: this.thumbnailFileSelected,
      deckName: this.deckName,
      deckDescription: this.deckDescription,
      deckVideoUrl: this.deckVideoUrl
    });
  }
}
