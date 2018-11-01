import { BrowserModule } from "@angular/platform-browser";
import { NgModule} from "@angular/core";
import {HomePage} from "./pages/home/home.page";
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {HttpClientModule} from "@angular/common/http";
import {SharedModule} from "../shared/shared.module";
import {RouterModule, Routes} from "@angular/router";

const homeRoutes: Routes = [
  { path: "home", component: HomePage},
];

@NgModule({
  declarations : [
    HomePage
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    RouterModule,
    RouterModule.forChild(homeRoutes)
  ],
  providers: []
})
export class HomeModule {}
