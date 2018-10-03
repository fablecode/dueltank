import {TipSection} from "./tipSection";

export class Card {
  public imageUrl: string;
  public name: string;
  public description: string;
  public cardNumber: string;
  public id: number = 0;
  public limit: string;
  public atk: string;
  public def: string;
  public types: string[];
  public baseType: string;

  public tipSections: TipSection[] = [];
}
