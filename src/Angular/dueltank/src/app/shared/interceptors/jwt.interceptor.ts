
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {Router} from "@angular/router";
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import {ErrorObservable} from "rxjs-compat/observable/ErrorObservable";
import {Globals} from "../../globals";
import {AuthenticationService} from "../services/authentication.service";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor (private route: Router, private globals: Globals, private auth: AuthenticationService) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next
      .handle(req)
      .do((ev: HttpEvent<any>) => {
        return next.handle(req);
      })
      .catch(response => {
        if (response instanceof HttpErrorResponse) {
          if(response.status === 401) {
            this.auth.logOut();
            this.route.navigate([this.globals.routes.login])
          }
        }

        return ErrorObservable.create(response);
      });
  }

}
