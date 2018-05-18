import {Component, OnInit} from "@angular/core";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {ActivatedRoute, Router} from "@angular/router";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";
import {FormControl, Validators} from "@angular/forms";

@Component({
  templateUrl: "./reset-password.page.html"
})
export class ResetPasswordPage implements OnInit {
  private email: FormControl;
  private password: FormControl;
  private confirmPassword: FormControl;
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

  }
}
