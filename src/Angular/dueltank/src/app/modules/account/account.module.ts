import {NgModule} from "@angular/core";
import {LoginPage} from "./pages/login/login.page";
import {RouterModule, Routes} from "@angular/router";
import {SocialLoginComponent} from "./components/socialLogin/socialLogin.component";
import {RegisterPage} from "./pages/register/register.page";

const accountRoutes: Routes = [
  {   path: "login", component: LoginPage},
  {   path: "register", component: RegisterPage}
];

@NgModule({
  declarations: [
    LoginPage,
    RegisterPage,
    SocialLoginComponent
  ],
  imports: [
    RouterModule.forChild(accountRoutes)
  ]
})
export class AccountModule {}
