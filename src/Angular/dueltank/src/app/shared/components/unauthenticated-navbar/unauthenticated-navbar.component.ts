import {Component} from "@angular/core";
import {Globals} from "../../../globals";

@Component({
  selector: "unauthenticated-navbar",
  templateUrl: "./unauthenticated-navbar.component.html"
})
export class UnAuthenticatedNavbarComponent {
  constructor(public globals: Globals) {

  }
}
