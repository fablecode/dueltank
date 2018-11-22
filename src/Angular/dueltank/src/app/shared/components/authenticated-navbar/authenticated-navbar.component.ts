import {Component} from "@angular/core";
import {UserProfile} from "../../models/userprofile";
import {UserProfileService} from "../../services/userprofile.service";
import {AuthenticationService} from "../../services/authentication.service";
import {Globals} from "../../../globals";

@Component({
  selector: "authenticated-navbar",
  templateUrl: "./authenticated-navbar.component.html"
})
export class AuthenticatedNavbarComponent {
  public user: UserProfile;

  constructor(private userProfileService: UserProfileService, private authService: AuthenticationService, public globals: Globals) {
    this.user = userProfileService.getUserProfile();
  }

  logOut() : void {
    this.authService.logOut();
  }
}
