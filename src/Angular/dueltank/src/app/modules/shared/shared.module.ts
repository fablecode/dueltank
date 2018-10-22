import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {LoadingSpinnerComponent} from "../../shared/components/loading-spinner/loading-spinner.component";
import {SlugifyPipe} from "../../shared/pipes/slugify.pipe";
import {DeckCurrentCardComponent} from "../deck/components/deck-current-card/deck-current-card.component";
import {DeckCurrentCardTextComponent} from "../deck/components/deck-current-card-text/deck-current-card-text.component";
import {TabsModule} from "ngx-bootstrap";
import {SafePipe} from "../../shared/pipes/safe.pipe";
import {DeckCurrentCardService} from "../deck/services/deck-current-card.service";

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    TabsModule.forRoot()
  ],
  declarations: [
    LoadingSpinnerComponent,
    DeckCurrentCardComponent,
    DeckCurrentCardTextComponent,
    SafePipe,
    SlugifyPipe
  ],
  exports: [
    DeckCurrentCardComponent,
    DeckCurrentCardTextComponent,
    LoadingSpinnerComponent
  ],
  providers:[
    DeckCurrentCardService
  ]
})
export class SharedModule {

}
