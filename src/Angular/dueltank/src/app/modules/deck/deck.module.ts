import {RouterModule, Routes} from "@angular/router";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {NgModule} from "@angular/core";
import {ModalModule, TabsModule} from "ngx-bootstrap";
import {DeckListPage} from "./pages/deck-list/deck-list.page";
import {DeckViewPage} from "./pages/deck-view/deck-view.page";
import {CardNameToUrlPipe} from "../../shared/pipes/cardNameToUrl.pipe";
import {DeckCurrentCard} from "./components/deck-current-card/deck-current-card";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {CommonModule} from "@angular/common";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BrowserModule} from "@angular/platform-browser";

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
    DeckCurrentCard,
    DeckListPage,
    DeckViewPage
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
