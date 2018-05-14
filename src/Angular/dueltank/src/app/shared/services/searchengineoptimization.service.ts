import {Injectable} from "@angular/core";
import {Meta, Title} from "@angular/platform-browser";

@Injectable()
export class SearchEngineOptimizationService {

  constructor(private titleService: Title, private metaService: Meta) {
  }

  public title(title: string) : void {
    this.titleService.setTitle(title);
  }

  public description(content: string) {
    this.metaService.addTag({name: "description", content: content})
  }
  public keywords(content: string) {
    this.metaService.addTag({name: "keywords", content: content})
  }
  public robots(content: string) {
    this.metaService.addTag({name: "robots", content: content})
  }

}
