import {Injectable} from "@angular/core";

@Injectable()
export class DeckRulesService {
  public static get numberOfCopies() : number { return 3; }
}
