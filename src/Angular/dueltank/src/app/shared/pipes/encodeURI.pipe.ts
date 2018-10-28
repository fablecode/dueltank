import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  name: 'encodeURI'
})
export class EncodeURI implements PipeTransform {

  public transform(uris: any, attribute: string) {
    if (uris === undefined) { return ''; }

    for (let item of uris) {
      item[attribute] = encodeURI(String(item[attribute]));
    }
    return uris;
  }
}
