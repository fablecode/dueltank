import {Component, OnInit} from "@angular/core";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {RegisterUser} from "../../../../shared/models/authentication/registeruser.model";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {ActivatedRoute} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: "register",
  templateUrl: "./register.page.html"
})
export class RegisterPage implements OnInit{
  private registerForm: FormGroup;
  private username: FormControl;
  private email: FormControl;
  private  password: FormControl;

  constructor(private authService: AuthenticationService, private activatedRoute: ActivatedRoute,){}

  ngOnInit() {
    this.createFormControls();
    this.createForm();
  }

  public onSubmit() {
    if(this.registerForm.valid) {

      var newUser = new RegisterUser();
      newUser.username = this.registerForm.controls.username.value;
      newUser.email = this.registerForm.controls.email.value;;
      newUser.password = this.registerForm.controls.password.value;;

      let returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'];

      this.authService.register(newUser, returnUrl)
        .subscribe(user => {}, error => this.handleError(error));

      console.log("register form submitted.")
      console.log(newUser);
    }
    else {
      //this.registerForm.
    }
  }

  private createFormControls() : void {
    this.username = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
      Validators.max(100)
    ]);
    this.email = new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.max(100),
      Validators.email
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

  private handleError(error: HttpErrorResponse) {
    
  }
}
