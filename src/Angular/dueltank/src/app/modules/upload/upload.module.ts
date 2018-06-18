import {NgModule} from "@angular/core";
import {RouterModule} from "@angular/router";
import {YgoproDeckPage} from "./pages/ygopro-deck/ygopro-deck.page";
import {AuthGuard} from "../../shared/guards/auth.guard";

const uploadRoutes = [
  {
    path: "upload",
    component: YgoproDeckPage,
    canActivateChild: [AuthGuard],
    children: [
      {   path: "ygopro-deck", component: YgoproDeckPage },
    ]
  }
]
@NgModule({
  declarations: [
    YgoproDeckPage
  ],
  imports: [
    RouterModule.forChild(uploadRoutes)
  ],
  providers: []
})

export class UploadModule {}
