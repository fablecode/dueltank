import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";
import {LoginUser} from "../../../../shared/models/authentication/loginuser.model";
import {Globals} from "../../../../globals";

@Component({
  templateUrl: "./login.page.html"
})
export class LoginPage implements OnInit {
  public email: FormControl;
  public password: FormControl;
  public rememberMe: FormControl;
  public loginForm: FormGroup;
  public httpValidationErrors: string[] = [];

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
    this.seo.title("DuelTank - Login");
    this.seo.description("Login to DuelTank either an existing user account or by using a social login.");
    this.seo.keywords("DuelTank, Login, Social Login")
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

    this.rememberMe = new FormControl("");
  }

  private createForm() {
    this.loginForm = new FormGroup({
      email: this.email,
      password: this.password,
      rememberMe: this.rememberMe
    })
  }

  public onSubmit() {
    if(this.loginForm.valid) {
      var existingUser = new LoginUser();

      existingUser.email = this.loginForm.controls.email.value;
      existingUser.password = this.loginForm.controls.password.value;
      existingUser.rememberMe = this.loginForm.controls.rememberMe.value === true;

      let returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || "";

      this.authService.login(existingUser, returnUrl)
        .subscribe(user => { return this.router.navigateByUrl(returnUrl); }, error => this.handleError(error));
    }
  }

  private handleError(httpError: HttpErrorResponse) {
    this.httpValidationErrors = httpError.error;
  }
}
