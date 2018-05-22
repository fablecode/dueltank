import {Component, OnInit} from "@angular/core";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserForgotPassword} from "../../../../shared/models/authentication/userforgotpassword.model";
import {Globals} from "../../../../globals";

@Component({
  templateUrl: "./forgotpassword.page.html"
})
export class ForgotPasswordPage implements OnInit{
  private email: FormControl;
  private forgotpassword: FormGroup;
  constructor
  (
    private seo: SearchEngineOptimizationService,
    private authService: AuthenticationService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private globals: Globals
  )
  {}

  ngOnInit(): void {
    this.seo.title("DuelTank - Forgot Password");
    this.seo.description("Reset your DuelTank password.");
    this.seo.keywords("DuelTank, Forgot, Password, Social Login")
    this.seo.robots("index,follow");

    this.createFormControls();
    this.createForm();
  }

  private createFormControls() {
    this.email = new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.max(100),
      Validators.email,
      Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")
    ]);
  }

  private createForm() {
    this.forgotpassword = new FormGroup({
      email: this.email,
    });
  }

  public onSubmit() {
    if(this.forgotpassword.valid) {
      var newForgotPassword = new UserForgotPassword();

      newForgotPassword.email = this.forgotpassword.controls.email.value;
      newForgotPassword.resetPasswordConfirmationUrl = window.location.origin + this.globals.urlSegments.resetPassword;

      this.authService
        .forgotPassword(newForgotPassword)
        .subscribe(response => { return this.router.navigate([this.globals.urlSegments.forgotPasswordConfirmation]);})
    }
  }
}
