import {Injectable} from "@angular/core";
import {AuthenticationService} from "../services/authentication.service";
import {ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs/Observable";
import {Globals} from "../../globals";

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild{
  constructor(private authService: AuthenticationService, private router: Router,  private globals: Globals) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    let url: string = state.url;

    return this.checkLogin(url);
  }

  private checkLogin(url: string) : boolean {
    if (this.authService.isLoggedIn().getValue()) { return true; }

    // Store the attempted URL for redirecting
    this.authService.redirectUrl = url;

    // Navigate to the login page with extras
    this.router.navigate(["/", this.globals.urlSegments.account, this.globals.urlSegments.login]);
    return false;
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.canActivate(childRoute, state);
  }
}
