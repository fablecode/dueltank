import {Injectable} from "@angular/core";

@Injectable()
export class TokenService {
  private token_key: string = "auth_token"

  constructor() {}

  public setAccessToken(token: string) : void {
    localStorage.setItem(this.token_key, token);
  }

  public getAccessToken() : string {
    return localStorage.getItem(this.token_key);
  }

  public hasToken() : boolean {
    return !!localStorage.getItem(this.token_key);
  }
}
