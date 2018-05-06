import { Component} from "@angular/core";
import {AuthenticationService} from "../../services/authentication.service";
import {Observable} from "rxjs/Observable";

@Component({
  selector: "navbar",
  templateUrl: "./navbar.component.html"
})
export class NavbarComponent {
  private isLoggedIn: Observable<boolean>;
  constructor(private authService: AuthenticationService) {
    this.isLoggedIn = authService.isLoggedIn();
  }
}
