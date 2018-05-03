import {Component} from "@angular/core";
import {APP_CONFIG} from "../../../../shared/services/app-config.service";

@Component({
  selector: "socialLogin",
  templateUrl: "./socialLogin.component.html"
})
export class SocialLoginComponent {
  openExternalLogin(provider: string) {
    window.location.href = APP_CONFIG.apiEndpoint + "/api/accounts/externallogin?provider=" + provider + "&returnUrl=" + window.location.origin + "/external-login-complete";
  }
}
