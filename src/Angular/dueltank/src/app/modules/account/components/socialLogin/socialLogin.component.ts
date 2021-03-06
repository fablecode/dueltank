import {Component} from "@angular/core";
import {AppConfigService} from "../../../../shared/services/app-config.service";
import {Globals} from "../../../../globals";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: "socialLogin",
  templateUrl: "./socialLogin.component.html"
})
export class SocialLoginComponent {
  public isLoading = false;
  constructor(private configuration: AppConfigService, private activatedRoute: ActivatedRoute, private globals: Globals){}
  openExternalLogin(provider: string) {
    this.isLoading = true;
    let returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || "";
    window.location.href = this.configuration.apiEndpoint + "/api/accounts/externallogin?provider=" + provider +
      "&returnUrl=" + returnUrl +
      "&loginUrl=" + window.location.origin + this.globals.routes.login +
      "&lockoutUrl=" + window.location.origin + this.globals.routes.lockout +
      "&externalLoginUrl=" + window.location.origin + this.globals.routes.externalLogin +
      "&externalLoginCompleteUrl=" + window.location.origin + this.globals.routes.externalLoginComplete;
  }
}
