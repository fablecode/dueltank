
import {AfterViewInit, Directive, ElementRef} from "@angular/core";
import * as $ from "jquery";
import "metismenu";

@Directive({
  selector: "[sideNavigation]"
})

export class SideNavigationDirective implements AfterViewInit {
  constructor(private el: ElementRef) {
  }

  ngAfterViewInit(): void {
    $(this.el.nativeElement).metisMenu();
  }
}
