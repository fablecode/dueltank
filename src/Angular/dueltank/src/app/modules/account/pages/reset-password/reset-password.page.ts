import {Component, OnInit} from "@angular/core";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {ActivatedRoute, Router} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";
import {ResetUserPassword} from "../../../../shared/models/reset-user-password";

@Component({
  templateUrl: "./reset-password.page.html"
})
export class ResetPasswordPage implements OnInit {
  public email: FormControl;
  public password: FormControl;
  public confirmPassword: FormControl;
  public resetPasswordForm: FormGroup;
  public httpValidationErrors: string[] = [];

  constructor(private seo: SearchEngineOptimizationService, private authService: AuthenticationService, private activatedRoute: ActivatedRoute, private router: Router){}

  ngOnInit(): void {
    this.seo.title("DuelTank - Reset Password");
    this.seo.description("Reset your DuelTank password.");
    this.seo.keywords("DuelTank, Reset, Password")
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
    this.password = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
      Validators.max(100)
    ]);

    this.confirmPassword = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
      Validators.max(100)
    ]);
  }

  private createForm() {
    this.resetPasswordForm = new FormGroup({
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword
    })
  }

  public onSubmit() {
    if(this.resetPasswordForm) {
      var resetUserPassword = new ResetUserPassword();

      resetUserPassword.email = this.resetPasswordForm.controls.email.value;
      resetUserPassword.password = this.resetPasswordForm.controls.password.value;
      resetUserPassword.confirmPassword = this.resetPasswordForm.controls.confirmPassword.value;
      resetUserPassword.code = this.activatedRoute.snapshot.queryParams.code || null;

      this.authService
        .resetPassword(resetUserPassword)
        .subscribe(user => { return this.router.navigate(["/reset-password-confirmation"]); }, error => this.handleError(error));
    }
  }

  private handleError(httpError: HttpErrorResponse) {
    this.httpValidationErrors = httpError.error;
  }
}
