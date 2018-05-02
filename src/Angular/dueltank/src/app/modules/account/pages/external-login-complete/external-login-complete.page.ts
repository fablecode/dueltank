import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {TokenService} from "../../../../shared/services/token.service";
import {AuthenticationService} from "../../../../shared/services/authentication.service";
import {UserProfileService} from "../../../../shared/services/userprofile.service";

@Component({
  templateUrl: "./external-login-complete.page.html"
})
export class ExternalLoginCompletePage implements OnInit {
  constructor
  (
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private tokenService: TokenService,
    private authService: AuthenticationService,
    private userProfileService: UserProfileService
  ) {}
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => console.log(params))

    var token = this.activatedRoute.snapshot.queryParams['token'];
    debugger;
    if(token) {
      this.tokenService.setAccessToken(token);

      this.authService.getProfile()
        .subscribe(data => {
          this.userProfileService.setUserProfile(data);
          return this.router.navigateByUrl("/");
        })
    }
    else {
      this.router.navigateByUrl("/");
    }
  }
}
