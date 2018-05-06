import {Component} from "@angular/core";
import {AppConfigService} from "../../../../shared/services/app-config.service";

@Component({
  selector: "socialLogin",
  templateUrl: "./socialLogin.component.html"
})
export class SocialLoginComponent {
  constructor(private configuration: AppConfigService){}
  openExternalLogin(provider: string) {
    window.location.href = this.configuration.apiEndpoint + "/api/accounts/externallogin?provider=" + provider + "&returnUrl=" + window.location.origin + "/external-login-complete";
  }
}
