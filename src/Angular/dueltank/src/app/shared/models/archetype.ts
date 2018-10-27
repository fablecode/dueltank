import DateTimeFormat = Intl.DateTimeFormat;

export class Archetype {
  public id: number;
  public name: string;
  public thumbnailUrl: string;
  public updated: DateTimeFormat;
  public totalCards: number;
}
