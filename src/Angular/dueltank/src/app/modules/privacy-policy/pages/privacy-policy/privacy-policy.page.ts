import {Component} from "@angular/core";
import {SearchEngineOptimizationService} from "../../../../shared/services/searchengineoptimization.service";

@Component({
  templateUrl: "./privacy-policy.page.html"
})
export class PrivacyPolicyPage {
  constructor(private seo: SearchEngineOptimizationService){
    seo.title("Privacy Policy - DuelTank")
  }
}
