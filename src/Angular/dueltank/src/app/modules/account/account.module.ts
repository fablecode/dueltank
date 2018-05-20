import {NgModule} from "@angular/core";
import {LoginPage} from "./pages/login/login.page";
import {RouterModule, Routes} from "@angular/router";
import {SocialLoginComponent} from "./components/socialLogin/socialLogin.component";
import {RegisterPage} from "./pages/register/register.page";
import {SignInLoadingComponent} from "../../shared/components/signin-loading/signin-loading.component";
import {ExternalLoginCompletePage} from "./pages/external-login-complete/external-login-complete.page";
import {CommonModule} from "@angular/common";
import {AuthenticationService} from "../../shared/services/authentication.service";
import {ModalModule} from "ngx-bootstrap";
import {AccountsService} from "../../shared/services/accounts.service";
import {UserProfileService} from "../../shared/services/userprofile.service";
import {TokenService} from "../../shared/services/token.service";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {SearchEngineOptimizationService} from "../../shared/services/searchengineoptimization.service";
import {ForgotPasswordPage} from "./pages/forgot-password/forgotpassword.page";
import {ForgotPasswordConfirmationPage} from "./pages/forgot-password-confirmation/forgot-password-confirmation.page";
import {ResetPasswordPage} from "./pages/reset-password/reset-password.page";
import {ResetPasswordConfirmationPage} from "./pages/reset-password-confirmation/reset-password-confirmation.page";

const accountRoutes: Routes = [
  {   path: "login", component: LoginPage},
  {   path: "register", component: RegisterPage},
  {   path: "external-login-complete", component: ExternalLoginCompletePage},
  {   path: "forgot-password", component: ForgotPasswordPage},
  {   path: "forgot-password-confirmation", component: ForgotPasswordConfirmationPage},
  {   path: "reset-password", component: ResetPasswordPage},
  {   path: "reset-password-confirmation", component: ResetPasswordConfirmationPage}
];

@NgModule({
  declarations: [
    LoginPage,
    RegisterPage,
    ExternalLoginCompletePage,
    SocialLoginComponent,
    SignInLoadingComponent,
    ForgotPasswordPage,
    ForgotPasswordConfirmationPage,
    ResetPasswordPage,
    ResetPasswordConfirmationPage
  ],
  imports: [
    CommonModule,
    FormsModule, ReactiveFormsModule,
    ModalModule.forRoot(),
    RouterModule.forChild(accountRoutes)
  ],
  providers: [
    AuthenticationService,
    TokenService,
    UserProfileService,
    AccountsService,
    SearchEngineOptimizationService
  ]
})
export class AccountModule {}
