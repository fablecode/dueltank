import { BrowserModule } from "@angular/platform-browser";
import { NgModule} from "@angular/core";
import {HomePage} from "./pages/home/home.page";
import {AlertModule} from "ngx-bootstrap";

@NgModule({
  declarations : [
    HomePage
  ],
  imports: [
    BrowserModule,
    AlertModule.forRoot()
  ],
  providers: []
})
export class HomeModule {}
