import {Pipe, PipeTransform} from "@angular/core";

@Pipe({name: "cardNameToUrl"})
export class CardNameToUrlPipe implements PipeTransform {
  public cardNames = /".*?"/g;

  transform(cardText: string): string {
    if(cardText.match(this.cardNames))
    {
      cardText = cardText.replace(cardText, function(cardNameMatch) {
        //replacing single quotes in card name
        cardNameMatch = cardNameMatch.replace(/'/g, "&#39;");

        return '<a (click)=\'cardSearchByName(' + cardNameMatch + ')\'>' + cardNameMatch + '</a>';
      });
    }

    return cardText;
  }
}
