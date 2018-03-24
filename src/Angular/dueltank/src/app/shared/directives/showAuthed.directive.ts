import {Directive, OnInit} from "@angular/core";

@Directive({
  selector: "[showAuthed]"
})

export class ShowAuthedDirective implements OnInit {
  ngOnInit(): void {
  }
}
