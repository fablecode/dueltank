import {NgModule} from "@angular/core";
import {LoginPage} from "./pages/login/login.page";
import {RouterModule, Routes} from "@angular/router";
import {SocialLoginComponent} from "./components/socialLogin/socialLogin.component";
import {RegisterPage} from "./pages/register/register.page";
import {ForgotPage} from "./pages/password/forgot/forgot.page";

const accountRoutes: Routes = [
  {   path: "login", component: LoginPage},
  {   path: "register", component: RegisterPage},
  {   path: "password/forgot", component: ForgotPage}
];

@NgModule({
  declarations: [
    LoginPage,
    RegisterPage,
    ForgotPage,
    SocialLoginComponent
  ],
  imports: [
    RouterModule.forChild(accountRoutes)
  ]
})
export class AccountModule {}
