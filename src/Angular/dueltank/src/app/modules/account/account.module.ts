import {NgModule} from "@angular/core";
import {LoginPage} from "./pages/login/login.page";
import {RouterModule, Routes} from "@angular/router";
import {SocialLoginComponent} from "./components/socialLogin/socialLogin.component";

const accountRoutes: Routes = [
  {   path: "login", component: LoginPage}
];

@NgModule({
  declarations: [
    LoginPage,
    SocialLoginComponent
  ],
  imports: [
    RouterModule.forChild(accountRoutes)
  ]
})
export class AccountModule {}
