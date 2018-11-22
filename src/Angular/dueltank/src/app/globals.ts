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
  routes = {
    uploadYgoProDeck: "/upload/ygopro-deck",
    login: "/account/login",
    register: "/account/register",
    forgotPassword: "/account/forgot-password",
    lockout: "/account/lockout",
    externalLogin: "/account/external-login",
    externalLoginComplete: "/account/external-login-complete",
    archetypes: "/archetypes",
    decks: "/decks",
    deck: {
      new: "/deck/new"
    },
    home: "/",
    banlist: {
      tcg: "/banlists/tcg/latest",
      ocg: "/banlists/ocg/latest"
    },
    user: {
      decks: "/user/decks"
    }
  }
}
