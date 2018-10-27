import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {ArchetypeListPage} from "./pages/archetype-list/archetype-list.page";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";
import {PaginationModule, TabsModule} from "ngx-bootstrap";
import {ArchetypeService} from "./services/archetype.service";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

const archetypeRoutes: Routes = [
  {
    path: "archetypes",
    component: ArchetypeListPage,
    // children: [
    //   {path:":id", component: LatestPage},
    //   {path:":id/:name", component: LatestPage}
    // ]
  }
];

@NgModule({
  declarations: [
    ArchetypeListPage
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    FormsModule,ReactiveFormsModule,
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    RouterModule.forChild(archetypeRoutes)
  ],
  providers: [
    ArchetypeService
  ]
})
export class ArchetypeModule {

}
