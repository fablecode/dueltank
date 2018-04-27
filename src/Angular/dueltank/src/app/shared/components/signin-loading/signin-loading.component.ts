import {Component, Input} from "@angular/core";
import {animate, style, transition, trigger} from "@angular/animations";

@Component({
  selector: "sign-loading",
  templateUrl: "signin-loading.component.html",
  styleUrls: ["signin-loading.component.css"],
  animations: [
    trigger(
      'leaveAnimation', [
        transition(':leave', [
          style({transform: 'translateX(0)', opacity: 1}),
          animate('500ms', style({transform: 'translateX(100%)', opacity: 0}))
        ])
      ]
    )
  ]
})
export class SignInLoadingComponent {
  @Input() loading: boolean = false;
}
