import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {ArchetypeListPage} from "./pages/archetype-list/archetype-list.page";
import {CommonModule} from "@angular/common";
import {SharedModule} from "../shared/shared.module";
import {PaginationModule, TabsModule} from "ngx-bootstrap";
import {ArchetypeService} from "./services/archetype.service";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ArchetypeViewPage} from "./pages/archetype-view/archetype-view.page";

const archetypeRoutes: Routes = [
  {path: "archetypes",component: ArchetypeListPage  },
  {
    path: "archetype",
    children: [
      {path:":id", component: ArchetypeViewPage},
      {path:":id/:name", component: ArchetypeViewPage}
    ]
  }
];

@NgModule({
  declarations: [
    ArchetypeListPage,
    ArchetypeViewPage
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
