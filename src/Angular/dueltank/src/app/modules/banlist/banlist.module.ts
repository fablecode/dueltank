import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {LatestPage} from "./pages/latest/latest.page";
import {BanlistService} from "./services/banlist.service";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";
import {TabsModule} from "ngx-bootstrap";

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
  declarations: [
    LatestPage
  ],
  imports: [
    CommonModule,
    SharedModule,
    TabsModule.forRoot(),
    RouterModule.forChild(banlistRoutes)],
  providers: [BanlistService]
})
export class BanlistModule {}
