import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse
} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {Router} from "@angular/router";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor (private route: Router) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .map(
      (event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          return event;
        }
      },
      (err) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            return this.route.navigateByUrl("/login");
          }
        }
      });
  }

}
