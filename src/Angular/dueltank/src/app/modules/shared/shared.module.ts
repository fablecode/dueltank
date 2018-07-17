import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {LoadingSpinnerComponent} from "../../shared/components/loading-spinner/loading-spinner.component";
import {SlugifyPipe} from "../../shared/pipes/slugify.pipe";

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule
  ],
  declarations: [
    LoadingSpinnerComponent,
    SlugifyPipe
  ],
  exports: [
    LoadingSpinnerComponent
  ]
})
export class SharedModule {

}
