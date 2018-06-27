import {RouterModule, Routes} from "@angular/router";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {NgModule} from "@angular/core";
import {ModalModule, TabsModule} from "ngx-bootstrap";
import {DeckListPage} from "./pages/deck-list/deck-list.page";
import {DeckViewPage} from "./pages/deck-view/deck-view.page";
import {CardNameToUrlPipe} from "../../shared/pipes/cardNameToUrl.pipe";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BrowserModule} from "@angular/platform-browser";
import {DeckCurrentCardComponent} from "./components/deck-current-card/deck-current-card.component";
import {DeckViewCardSearchComponent} from "./components/deck-view-card-search/deck-view-card-search.component";
import {DeckViewFormOptionsComponent} from "./components/deck-view-form-options/deck-view-form-options.component";

const deckRoutes: Routes = [
  {
    path: "decks",
    children: [
      { path: '', component: DeckListPage },
      { path: ":id/:name", component: DeckViewPage}
    ]
  }
];

@NgModule({
  declarations: [
    CardNameToUrlPipe,
    DeckCurrentCardComponent,
    DeckListPage,
    DeckViewPage,
    DeckViewCardSearchComponent,
    DeckViewFormOptionsComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule, ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    RouterModule.forChild(deckRoutes)
  ],
  providers: [
    AuthGuard
  ]
})
export class DeckModule {}
