
import {AfterViewInit, Directive, ElementRef} from "@angular/core";
import "metismenu";
import * as $ from "jquery";

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
