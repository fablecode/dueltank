import {Component, Input} from "@angular/core";
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: "loadingSpinner",
  templateUrl: "loading-spinner.component.html",
  styleUrls: ["loading-spinner.component.css"],
  animations: [
    trigger(
      'leaveAnimation', [
        transition(':leave', [
          style({ opacity: 1 }),
          animate('0.2s', style({ opacity: 0 }))
        ])
      ]
    )
  ]
})
export class LoadingSpinnerComponent {
  @Input() loading: boolean = false;
}
