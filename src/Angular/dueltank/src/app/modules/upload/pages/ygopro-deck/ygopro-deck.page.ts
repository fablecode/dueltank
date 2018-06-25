import {Component, EventEmitter, OnInit, Output} from "@angular/core";
import {FileQueueObject, FileUploaderService} from "../../../../shared/services/file-upload.service";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {Observable} from "rxjs/Observable";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";


@Component({
  templateUrl: "./ygopro-deck.page.html"
})
export class YgoProDeckPage implements OnInit {
  public filesSelected: FormControl;
  public deckUploadForm: FormGroup;

  public queue: Observable<FileQueueObject[]>;
  @Output() onCompleteItem = new EventEmitter();

  constructor(public uploader: FileUploaderService, private fb: FormBuilder, private seo: SearchEngineOptimizationService) {
    this.seo.title("DuelTank - Upload YgoPro decks");
    this.seo.description("Upload your favourite YgoPro decks");
    this.seo.keywords("DuelTank, deck, upload")
    this.seo.robots("index,follow");
  }

  public onDeckSelected(files: FileList) {
    this.uploader.addToQueue(files);

    if(files.length > 1) {
      this.filesSelected.setValue(files.length + " files selected.");
    }
    else {
      this.filesSelected.setValue(files.item(0).name);
    }
  }

  public onSubmit() {
    this.uploader.uploadAll();
  }

  public completeItem = (item: FileQueueObject, response: any) => {
    this.onCompleteItem.emit({ item, response });
  }

  ngOnInit(): void {

    this.queue = this.uploader.queue;
    this.uploader.onCompleteItem = this.completeItem;

    // create form
    this.filesSelected = new FormControl("", [
      Validators.required
    ]);

    this.deckUploadForm = this.fb.group({
      file: this.filesSelected
    })
  }
}




