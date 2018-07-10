import {Component, OnInit} from "@angular/core";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {RegisterUser} from "../../../../shared/models/authentication/registeruser.model";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {ActivatedRoute, Router} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {Globals} from "../../../../globals";
import {existingUsernameValidator} from "../../validators/existingUsernameValidator";

@Component({
  templateUrl: "./register.page.html"
})
export class RegisterPage implements OnInit{
  public registerForm: FormGroup;
  public username: FormControl;
  public email: FormControl;
  public password: FormControl;
  public httpValidationErrors: string[] = [];

  constructor
  (
    private seo: SearchEngineOptimizationService,
    private authService: AuthenticationService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public globals: Globals
  )
  {}

  ngOnInit() {
    this.seo.title("DuelTank - Register");
    this.seo.description("Register with DuelTank by either creating a new account or using a social media login.");
    this.seo.keywords("DuelTank, Register, Social Login")
    this.seo.robots("index,follow");

    this.createFormControls();
    this.createForm();
  }

  public onSubmit() {
    if(this.registerForm.valid) {

      var newUser = new RegisterUser();
      newUser.username = this.registerForm.controls.username.value;
      newUser.email = this.registerForm.controls.email.value;
      newUser.password = this.registerForm.controls.password.value;;

      let returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || "";

      this.authService.register(newUser, returnUrl)
        .subscribe(user => { return this.router.navigateByUrl(returnUrl); }, error => this.handleError(error));
    }
  }

  private createFormControls() : void {
    this.username = new FormControl("",
      [
      Validators.required,
      Validators.minLength(4),
      Validators.max(100)
    ],
      [
        existingUsernameValidator(this.authService)
      ]);
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
  }

  private createForm() : void {
    this.registerForm = new FormGroup({
      username: this.username,
      email: this.email,
      password: this.password
    })
  }

  private handleError(httpError: HttpErrorResponse) {
    this.httpValidationErrors = httpError.error;
  }
}
