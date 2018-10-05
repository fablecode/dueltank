import {Pipe, PipeTransform} from "@angular/core";
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Pipe({name: "cardNameToUrl"})
export class CardNameToUrlPipe implements PipeTransform {
  public cardNames = /".*?"/g;

  constructor(private domSanitizer: DomSanitizer){}

  transform(cardText: string): SafeHtml {
    if(cardText.match(this.cardNames))
    {
      cardText = cardText.replace(this.cardNames, function(cardNameMatch) {
        //replacing single quotes in card name
        cardNameMatch = cardNameMatch.replace(/'/g, "&#39;");

        return this.domSanitizer.bypassSecurityTrustHtml('<a (click)=\'cardSearchByName(' + cardNameMatch + ')\'>' + cardNameMatch + '</a>');
      });
    }

    return cardText;
  }
}
