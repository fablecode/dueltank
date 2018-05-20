import {Injectable} from "@angular/core";

@Injectable()
export class Globals {
  appRoutes = {
    login: "/account/login",
    register: "/account/register"
  };
}
