import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";
import {RouterModule, Routes} from "@angular/router";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {PaginationModule, TabsModule} from "ngx-bootstrap";
import {UserDeckListPage} from "./pages/decks/user-deck-list.page";


const userRoutes: Routes = [
  {
    path: "user",
    children:[
      {path:"decks", component: UserDeckListPage},
      // { path: "editor/:id/:name", component: DeckEditorPage}
    ]
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
    UserDeckListPage
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
