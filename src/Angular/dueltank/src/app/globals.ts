import {Injectable} from "@angular/core";

@Injectable()
export class Globals {
  urlSegments = {
    account: "account",
    login: "login",
    register: "register",
    forgotPassword: "forgot-password",
    forgotPasswordConfirmation: "forgot-password-confirmation",
    resetPassword: "reset-password",
    resetPasswordConfirmation: "reset-password-confirmation",
    externalLoginComplete: "external-login-complete",
    upload: "upload",
    ygopro: "ygopro",
    decks: "decks"
  };
}
