import {Injectable} from "@angular/core";
import {Meta, MetaDefinition, Title} from "@angular/platform-browser";

@Injectable()
export class SearchEngineOptimizationService {

  constructor(private titleService: Title, private metaService: Meta) {
  }

  public setTitle(title: string) : void {
    this.titleService.setTitle(title);
  }

  public addMetaTag(tag: MetaDefinition) : void {
    this.metaService.addTag(tag);
  }

  public addMetaTags(tags: MetaDefinition[]) : void {
    this.metaService.addTags(tags)
  }
}
