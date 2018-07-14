import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {TokenService} from "../../../../shared/services/token.service";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {existingUsernameValidator} from "../../validators/existingUsernameValidator";
import {ExternalLoginConfirmation} from "../../../../shared/models/authentication/externalloginconfirmation.model";
import {UserProfile} from "../../../../shared/models/userprofile";

@Component({
  templateUrl: "./external-login.page.html"
})
export class ExternalLoginPage implements OnInit {
  public provider: string;
  private returnUrl: string;
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

    this.provider = this.activatedRoute.snapshot.queryParams['provider'];
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];

    this.createFormControls();
    this.createForm();
  }

  public onSubmit() {
    var model = new ExternalLoginConfirmation();
    model.username = this.username.value;
    this.authService.externalLoginConfirmation(model)
      .subscribe
      (
        () => {
          if(this.returnUrl) {

            this.router.navigateByUrl(this.returnUrl);
          }
          else {
            this.router.navigateByUrl("/");
          }
        }
      )
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
