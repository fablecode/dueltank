import {Card} from "./card";

export class Deck {
  public id: string;
  public username: string;
  public thumbnailUrl: string;
  public name: string;
  public description: string;
  public videoId: string;
  public youtubeUrl: string;
  public totalCards: string;
  public createdTimeAgo: string;
  public updatedTimeAgo: string;
  public created: Date;
  public updated: Date;
  public mainDeck: Card[] = [];
  public extraDeck: Card[] = [];
  public sideDeck: Card[] = [];
}
