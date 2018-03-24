import { BrowserModule } from "@angular/platform-browser";
import { NgModule} from "@angular/core";
import {HomePage} from "./pages/home/home.page";

@NgModule({
  declarations : [
    HomePage
  ],
  imports: [
    BrowserModule
  ],
  providers: []
})
export class HomeModule {}
