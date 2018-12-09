import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";
import {RouterModule, Routes} from "@angular/router";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {PaginationModule, TabsModule} from "ngx-bootstrap";
import {UserDeckListPage} from "./pages/user-deck-list/user-deck-list.page";
import {UsernameDeckListPage} from "./pages/username-deck-list/username-deck-list.page";
import {UserDecksService} from "./services/user-decks.service";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {BrowserModule} from "@angular/platform-browser";


const userRoutes: Routes = [
  {
    path: "user",
    children:[
      {path:"decks", component: UserDeckListPage},
      {path:":username/decks", component: UsernameDeckListPage}
    ]
  }
];

@NgModule({
  declarations: [
    UserDeckListPage,
    UsernameDeckListPage
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    FormsModule,ReactiveFormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    RouterModule.forChild(userRoutes)
  ],
  providers: [
    UserDecksService
  ]
})
export class UserModule {}
