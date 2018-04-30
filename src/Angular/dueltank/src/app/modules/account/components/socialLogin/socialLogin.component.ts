import {Component, OnInit, TemplateRef} from "@angular/core";
import {BsModalRef, BsModalService} from "ngx-bootstrap";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {Externallogin} from "../../../../shared/models/externallogin";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: "socialLogin",
  templateUrl: "./socialLogin.component.html"
})
export class SocialLoginComponent implements OnInit {
  private modalRef: BsModalRef;
  private providerUrl: string;
  private authWindow: Window;

  constructor(private authService: AuthenticationService, private modalService: BsModalService, private route: ActivatedRoute) {}

  openExternalLogin(provider: string) {
    // let w: number = 700;
    // let h: number = 750;
    // var y = window.top.outerHeight / 2 + window.top.screenY - ( h / 2)
    // var x = window.top.outerWidth / 2 + window.top.screenX - ( w / 2)
    // this.authWindow =  window.open("http://localhost:56375/api/accounts/externallogin?provider=" + provider, provider, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+y+', left='+x);

    window.location.href = "http://localhost:56375/api/accounts/externallogin?provider=" + provider + "&returnUrl=http://localhost:4200/external-login-complete";

    //this.modalRef = this.modalService.show(template);
    // var externalLogin = new Externallogin();
    //
    //  externalLogin.provider = provider;
    //  externalLogin.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    //
    //  this.authService
    //    .externalLogin(externalLogin)
    //    .subscribe(resp => {
    //      let providerAuthenticationUrl = resp.headers.get("Location");
    //
    //      console.log(providerAuthenticationUrl)
    //    })
    //
    //  console.log(externalLogin);
  }

  ngOnInit(): void {

  }
}
