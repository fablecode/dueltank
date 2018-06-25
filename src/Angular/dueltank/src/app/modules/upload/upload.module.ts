import {NgModule} from "@angular/core";
import {RouterModule} from "@angular/router";
import { YgoProDeckPage} from "./pages/ygopro-deck/ygopro-deck.page";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {CommonModule} from "@angular/common";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FileUploaderService} from "../../shared/services/file-upload.service";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {FileSizePipe} from "../../shared/pipes/filesize.pipe";
import {TooltipModule} from "ngx-bootstrap";

const uploadRoutes = [
  {
    path: "upload",
    canActivateChild: [AuthGuard],
    children: [
      {   path: "ygopro-deck", component: YgoProDeckPage},
    ]
  }
]
@NgModule({
  declarations: [
    FileSizePipe,
    YgoProDeckPage
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule, ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    TooltipModule.forRoot(),
    RouterModule.forChild(uploadRoutes)
  ],
  providers: [
    FileUploaderService
  ]
})

export class UploadModule {}
