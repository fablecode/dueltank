import {Component, EventEmitter, Output} from "@angular/core";
import {FileUploaderService} from "../../../../shared/services/file-upload.service";


@Component({
  templateUrl: "./ygopro-deck.page.html"
})
export class YgoProDeckPage {
  @Output() onCompleteItem = new EventEmitter();

  constructor(public uploader: FileUploaderService) { }

  public onDeckSelected(files: FileList) {
    //this.uploader.addToQueue(files);
    console.log(files);
  }
}




