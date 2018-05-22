import {Component} from "@angular/core";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {Globals} from "../../../../globals";

@Component({
  selector: "socialLogin",
  templateUrl: "./socialLogin.component.html"
})
export class SocialLoginComponent {
  constructor(private configuration: AppConfigService, private globals: Globals){}
  openExternalLogin(provider: string) {
    window.location.href = this.configuration.apiEndpoint + "/api/accounts/externallogin?provider=" + provider + "&returnUrl=" + window.location.origin + "/" + this.globals.urlSegments.account + "/" + this.globals.urlSegments.externalLoginComplete;
  }
}
