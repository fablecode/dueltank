import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: "register",
  templateUrl: "./register.page.html"
})
export class RegisterPage implements OnInit{
  private registerForm: FormGroup;
  private username: FormControl;
  private email: FormControl;
  private  password: FormControl;

  constructor(){}

  ngOnInit() {
    this.createFormControls();
    this.createForm();
  }

  private createFormControls() : void {
    this.username = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
      Validators.max(100)
    ])
    this.email = new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.max(100),
      Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
    ]);
    this.password = new FormControl("", [
      Validators.required,
      Validators.minLength(6),
      Validators.max(100)
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
