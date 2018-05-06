import {Component} from "@angular/core";
import {UserProfile} from "../../models/userprofile";
import {UserProfileService} from "../../services/userprofile.service";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: "authenticated-navbar",
  templateUrl: "./authenticated-navbar.component.html"
})
export class AuthenticatedNavbarComponent {
  user: UserProfile;

  constructor(private userProfileService: UserProfileService, private authService: AuthenticationService) {
    this.user = userProfileService.getUserProfile();
  }

  logOut() : void {
    this.authService.logOut();
  }
}
