import {NgModule} from "@angular/core";
import {LoginPage} from "./pages/login/login.page";
import {RouterModule, Routes} from "@angular/router";
import {SocialLoginComponent} from "./components/socialLogin/socialLogin.component";
import {RegisterPage} from "./pages/register/register.page";
import {ForgotPage} from "./pages/password/forgot/forgot.page";
import {ResetPage} from "./pages/password/reset/reset.page";
import {SignInLoadingComponent} from "../../shared/components/signin-loading/signin-loading.component";
import {SignInFacebookPage} from "./pages/signin-facebook/signin-facebook.page";
import {CommonModule} from "@angular/common";

const accountRoutes: Routes = [
  {   path: "login", component: LoginPage},
  {   path: "register", component: RegisterPage},
  {   path: "password/forgot", component: ForgotPage},
  {   path: "password/reset", component: ResetPage},
  {   path: "signin-facebook", component: SignInFacebookPage}
];

@NgModule({
  declarations: [
    LoginPage,
    RegisterPage,
    ForgotPage,
    ResetPage,
    SignInFacebookPage,
    SocialLoginComponent,
    SignInLoadingComponent
  ],
  imports: [
    RouterModule.forChild(accountRoutes)
  ]
})
export class AccountModule {}
