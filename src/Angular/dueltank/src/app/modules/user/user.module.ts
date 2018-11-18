import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";
import {RouterModule, Routes} from "@angular/router";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {PaginationModule, TabsModule} from "ngx-bootstrap";
import {UserDeckListPage} from "./pages/user-deck-list/user-deck-list.page";
import {UsernameDeckListPage} from "./pages/username-deck-list/username-deck-list.page";


const userRoutes: Routes = [
  {
    path: "user",
    children:[
      {path:"decks", component: UserDeckListPage},
      {path:":username/decks", component: UsernameDeckListPage},
      // { path: "editor/:id/:name", component: DeckEditorPage}
    ],

  },
  // {
  //   path: "archetype",
  //   children: [
  //     {path:":id", component: ArchetypeViewPage},
  //     {path:":id/:name", component: ArchetypeViewPage}
  //   ]
  // }
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
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    RouterModule.forChild(userRoutes)
  ],
  providers: []
})
export class UserModule {}
