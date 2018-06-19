import {NgModule} from "@angular/core";
import {RouterModule} from "@angular/router";
import { YgoProDeckPage} from "./pages/ygopro-deck/ygopro-deck.page";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {CommonModule} from "@angular/common";
import {BrowserModule} from "@angular/platform-browser";
import {HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {FileUploaderService} from "../../shared/services/file-upload.service";

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
    YgoProDeckPage
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule.forChild(uploadRoutes)
  ],
  providers: [
    FileUploaderService
  ]
})

export class UploadModule {}
