import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {PrivacyPolicyPage} from "./pages/privacy-policy/privacy-policy.page";
import {RouterModule, Routes} from "@angular/router";

const contactRoutes: Routes = [
  {   path: "privacy", component: PrivacyPolicyPage}
]

@NgModule({
  declarations : [
    PrivacyPolicyPage
  ],
  imports: [
    BrowserModule,
    RouterModule.forChild(contactRoutes)
  ],
  providers: []
})
export class PrivatePolicyModule {}
