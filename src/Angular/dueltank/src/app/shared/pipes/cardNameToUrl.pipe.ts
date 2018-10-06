import {ElementRef, Pipe, PipeTransform, Renderer2} from "@angular/core";
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Pipe({name: "cardNameToUrl"})
export class CardNameToUrlPipe implements PipeTransform {
  public cardNames = /".*?"/g;

  constructor(private domSanitizer: DomSanitizer, private elementRef: ElementRef, private renderer: Renderer2){}

  transform(cardText: string): SafeHtml {
    if(cardText.match(this.cardNames))
    {
      console.log(this.elementRef);

      cardText = cardText.replace(this.cardNames, function(cardNameMatch) {
        //replacing single quotes in card name
        cardNameMatch = cardNameMatch.replace(/'/g, "&#39;");

        return '<a (click)=\'cardSearchByName(' + cardNameMatch + ')\'>' + cardNameMatch + '</a>';
      });
    }

    return this.domSanitizer.bypassSecurityTrustHtml(cardText);
  }
}
