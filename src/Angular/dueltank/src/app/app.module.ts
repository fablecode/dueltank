import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import {HomeModule} from "./modules/home/home.module";
import {RouterModule, Routes} from "@angular/router";
import {HomePage} from "./modules/home/pages/home/home.page";
import {NavbarComponent} from "./shared/components/navbar/navbar.component";
import {SideNavigationComponent} from "./shared/components/sidenavigation/sidenavigation.component";
import {SideNavigationDirective} from "./shared/directives/sideNavigation.directive";
import {AccountModule} from "./modules/account/account.module";
import {ShowAuthedDirective} from "./shared/directives/showAuthed.directive";

const appRoutes: Routes = [
  {   path: "", component: HomePage, pathMatch: "full"}
];

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SideNavigationComponent,
    SideNavigationDirective,
    ShowAuthedDirective
  ],
  imports: [
    BrowserModule,
    HomeModule,
    AccountModule,
    RouterModule.forRoot(appRoutes)
  ],
  exports: [RouterModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
