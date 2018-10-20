import {fusionType, linkType, monsterType, spellType, synchroType, trapType, xyzType} from "./deck.constants";

export const mainDeckSize: number  = 60;
export const extraDeckSize: number  = 15;
export const sideDeckSize: number  = 15;

export const mainDeckAllowCardTypes : string[] = [monsterType, spellType, trapType];
export const extraDeckAllowCardTypes : string[] = [fusionType, synchroType, xyzType, linkType];
