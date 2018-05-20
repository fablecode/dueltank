import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot} from "@angular/router";
import {AuthenticationService} from "../services/authentication.service";
import {Observable} from "rxjs/Observable";

@Injectable()
export class AccountGuard implements CanActivate, CanActivateChild{
  constructor(private authService: AuthenticationService, private router: Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.checkLogin();
  }

  private checkLogin() : boolean {
    let isLoggedIn: boolean;
    this.authService.isLoggedIn().subscribe(loggedIn => { isLoggedIn = loggedIn;});

    if (isLoggedIn) {
      this.router.navigate(['/']);
    }

    return !isLoggedIn
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    return this.canActivate(childRoute, state);
  }
}
