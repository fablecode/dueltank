import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {LatestPage} from "./pages/latest/latest.page";
import {BanlistService} from "./services/banlist.service";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";

const banlistRoutes: Routes = [
  {
    path: "banlists",
    children: [
      {
        path:":format",
        children:[
          { path:"latest", component: LatestPage }
        ]
      }
    ]
  }
];

@NgModule({
  declarations: [LatestPage],
  imports: [CommonModule, SharedModule, RouterModule.forChild(banlistRoutes)],
  providers: [BanlistService]
})
export class BanlistModule {}
