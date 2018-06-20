import {Component, EventEmitter, OnInit, Output} from "@angular/core";
import {FileUploaderService} from "../../../../shared/services/file-upload.service";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";


@Component({
  templateUrl: "./ygopro-deck.page.html"
})
export class YgoProDeckPage implements OnInit {
  public filesSelected: FormControl;
  public deckUploadForm: FormGroup;
  @Output() onCompleteItem = new EventEmitter();

  constructor(public uploader: FileUploaderService, private fb: FormBuilder) { }

  public onDeckSelected(files: FileList) {
    //this.uploader.addToQueue(files);
    console.log(files);

    if(files.length > 1) {
      this.filesSelected.setValue(files.length + " files selected.");
    }
    else {
      this.filesSelected.setValue(files.item(0).name);
    }
  }

  public onSubmit() {

  }

  ngOnInit(): void {
    this.filesSelected = new FormControl("", [
      Validators.required
    ]);

    this.deckUploadForm = this.fb.group({
      file: this.filesSelected
    })
  }
}




