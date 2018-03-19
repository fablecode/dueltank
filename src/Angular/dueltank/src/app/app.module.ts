import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import {HomeModule} from "./modules/home/home.module";
import {RouterModule, Routes} from "@angular/router";
import {HomePage} from "./modules/home/pages/home/home.page";
import {NavbarComponent} from "./shared/components/navbar.component";

const appRoutes: Routes = [
  {   path: "", component: HomePage, pathMatch: "full"}
];

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent
  ],
  imports: [
    BrowserModule,
    HomeModule,
    RouterModule.forRoot(appRoutes)
  ],
  exports: [RouterModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
