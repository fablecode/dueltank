import {RouterModule, Routes} from "@angular/router";
import {AuthGuard} from "../../shared/guards/auth.guard";
import {NgModule} from "@angular/core";
import {ModalModule} from "ngx-bootstrap";
import {DeckListPage} from "./pages/deck-list/deck-list.page";

const deckRoutes: Routes = [
  {
    path: "decks",
    component: DeckListPage,
    canActivateChild: [AuthGuard],
    children: [
    ]
  }
];

@NgModule({
  declarations: [
    DeckListPage,
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
