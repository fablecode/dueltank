import {Component} from "@angular/core";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: "register",
  templateUrl: "./register.page.html"
})
export class RegisterPage {
  private registerForm: FormGroup;
  private username: FormControl;
  private email: FormControl;
  private  password: FormControl;

  constructor(private formBuilder: FormBuilder){
    this.createFormControls();
    this.createForm();
  }

  private createFormControls() : void {
    this.username = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
    ])
    this.email = new FormControl('', [
      Validators.required,
      Validators.pattern("[^ @]*@[^ @]*")
    ]);
    this.password = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
    ])
  }

  private createForm() : void {
    this.registerForm = new FormGroup({
      username: this.username,
      email: this.email,
      password: this.password
    })
  }
}
