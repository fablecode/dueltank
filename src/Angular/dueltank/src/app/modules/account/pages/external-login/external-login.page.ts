import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {TokenService} from "../../../../shared/services/token.service";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {existingUsernameValidator} from "../../validators/existingUsernameValidator";

@Component({
  templateUrl: "./external-login-complete.page.html"
})
export class ExternalLoginCompletePage implements OnInit {
  public provider: string;
  public registerExternalUserForm: FormGroup;
  public username: FormControl;

  constructor
  (
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private tokenService: TokenService,
    private authService: AuthenticationService) {

  }
  ngOnInit(): void {

    var token = this.activatedRoute.snapshot.queryParams['token'];
    this.provider = this.activatedRoute.snapshot.queryParams['provider'];

    this.createFormControls();
    this.createForm();
    // if(token) {
    //
    //   this.authService.externalLoginCallback(token)
    //     .subscribe(data => {
    //       return this.router.navigateByUrl("/");
    //     });
    // }
    // else {
    //   this.router.navigateByUrl("/");
    // }
  }

  private createFormControls() {
    this.username = new FormControl("",
      [
        Validators.required,
        Validators.minLength(4),
        Validators.max(100)
      ],
      [
        existingUsernameValidator(this.authService)
      ]);
  }

  private createForm() {
    this.registerExternalUserForm = new FormGroup({
      username: this.username
    })
  }
}
