import {NgModule} from "@angular/core";
import { ContactComponent } from './pages/contact/contact.component';
import {ContactService} from "./services/contact.service";
import {RouterModule, Routes} from "@angular/router";
import {CommonModule} from "@angular/common";
import {BrowserModule} from "@angular/platform-browser";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {HttpClientModule} from "@angular/common/http";
import {SharedModule} from "../shared/shared.module";

const contactRoutes: Routes = [
  {path: "contact", component: ContactComponent}
];

@NgModule({
  declarations: [
    ContactComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule, ReactiveFormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    RouterModule,
    RouterModule.forChild(contactRoutes)
  ],
  providers: [ContactService]
})
export class ContactModule {}
