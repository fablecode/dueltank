import { Component} from "@angular/core";
import {AuthenticationService} from "../../services/authentication.service";
import {Observable} from "rxjs";

@Component({
  selector: "sidenavigation",
  templateUrl: "./sidenavigation.component.html"
})

export class SideNavigationComponent {
  isLoggedIn: Observable<boolean>;

  constructor(private authService: AuthenticationService) {
    this.isLoggedIn = authService.isLoggedIn();
  }
}
