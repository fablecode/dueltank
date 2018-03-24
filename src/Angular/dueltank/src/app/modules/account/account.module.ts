import {NgModule} from "@angular/core";
import {LoginPage} from "./pages/login/login.page";
import {RouterModule, Routes} from "@angular/router";
import {SocialLoginComponent} from "./components/socialLogin/socialLogin.component";
import {RegisterPage} from "./pages/register/register.page";
import {ForgotPage} from "./pages/password/forgot/forgot.page";
import {ResetPage} from "./pages/password/reset/reset.page";

const accountRoutes: Routes = [
  {   path: "login", component: LoginPage},
  {   path: "register", component: RegisterPage},
  {   path: "password/forgot", component: ForgotPage},
  {   path: "password/reset", component: ResetPage}
];

@NgModule({
  declarations: [
    LoginPage,
    RegisterPage,
    ForgotPage,
    ResetPage,
    SocialLoginComponent
  ],
  imports: [
    RouterModule.forChild(accountRoutes)
  ]
})
export class AccountModule {}
