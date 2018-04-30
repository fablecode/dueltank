import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";

@Component({
  templateUrl: "./external-login-complete.page.html"
})
export class ExternalLoginCompletePage implements OnInit {
  constructor(private route: ActivatedRoute) {

  }
  ngOnInit(): void {
    this.route.params.subscribe(params => console.log(params))

    console.log(this.route.snapshot.queryParams['token']);
  }
}
