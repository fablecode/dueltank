import {RouterModule, Routes} from "@angular/router";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {NgModule} from "@angular/core";
import {ModalModule} from "ngx-bootstrap";
import {DeckListPage} from "./pages/deck-list/deck-list.page";
import {DeckViewPage} from "./pages/deck-view/deck-view.page";

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
    DeckListPage,
    DeckViewPage
  ],
  imports: [
    ModalModule.forRoot(),
    RouterModule.forChild(deckRoutes)
  ],
  providers: [
    AuthGuard
  ]
})
export class DeckModule {}
