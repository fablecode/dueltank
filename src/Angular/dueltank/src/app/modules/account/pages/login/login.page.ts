import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";

@Component({
  templateUrl: "./login.page.html"
})
export class LoginPage implements OnInit {
  title = "Login";

  constructor(private seo: SearchEngineOptimizationService, private authService: AuthenticationService, private activatedRoute: ActivatedRoute, private router: Router){}

  ngOnInit(): void {
    this.seo.setTitle("DuelTank - Login");

  }
}
