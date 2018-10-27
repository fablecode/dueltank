import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {LoadingSpinnerComponent} from "../../shared/components/loading-spinner/loading-spinner.component";
import {SlugifyPipe} from "../../shared/pipes/slugify.pipe";
import {CurrentHoverCardComponent} from "../../shared/components/current-hover-card/current-hover-card.component";
import {CurrentHoverCardTextComponent} from "../../shared/components/current-hover-card-text/current-hover-card-text.component";
import {TabsModule} from "ngx-bootstrap";
import {SafePipe} from "../../shared/pipes/safe.pipe";
import {CurrentHoverCardService} from "../deck/services/current-hover-card.service";

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    TabsModule.forRoot()
  ],
  declarations: [
    LoadingSpinnerComponent,
    CurrentHoverCardComponent,
    CurrentHoverCardTextComponent,
    SafePipe,
    SlugifyPipe
  ],
  exports: [
    CurrentHoverCardComponent,
    CurrentHoverCardTextComponent,
    LoadingSpinnerComponent,
    SlugifyPipe
  ],
  providers:[
    CurrentHoverCardService
  ]
})
export class SharedModule {

}
