import {Component, Input, OnChanges, OnInit, SimpleChanges} from "@angular/core";
import {DeckCurrentCardTextService} from "../../../modules/deck/services/deck-current-card-text.service";

@Component({
  selector: "currentHoverCardText",
  templateUrl: "./current-hover-card-text.component.html"
})
export class CurrentHoverCardTextComponent implements OnChanges {
  @Input() text: string;
  @Input() textToHtml: false;
  public cardNames = /".*?"/g;

  constructor(private deckCurrentCardTextService : DeckCurrentCardTextService) {}


  public cardSearchByName(cardName: string) : void {
    console.log("cardSearchByName invoked");
    console.log(cardName);
    this.deckCurrentCardTextService.onCardTextClick(cardName);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(this.textToHtml && this.text.match(this.cardNames))
    {
      let cardTextReplace = changes.text.currentValue.replace(this.cardNames, function(cardNameMatch) {
        //replacing single quotes in card name
        cardNameMatch = cardNameMatch.replace(/'/g, "&#39;");

        return '<a cardName=' + cardNameMatch + '\'>' + cardNameMatch + '</a>';
      });

      this.text = cardTextReplace;
    }

  }
}
