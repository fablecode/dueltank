import {Pipe, PipeTransform} from "@angular/core";

@Pipe({name: "filesize"})
export class FileSizePipe implements PipeTransform {
  transform(value: number): string {
    if (isNaN(value) || value < 0)
      value = 0;

    if (value < 1024)
      return value + ' Bytes';

    value /= 1024;

    if (value < 1024)
      return value.toFixed(2) + ' Kb';

    value /= 1024;

    if (value < 1024)
      return value.toFixed(2) + ' Mb';

    value /= 1024;

    if (value < 1024)
      return value.toFixed(2) + ' Gb';

    value /= 1024;

    return value.toFixed(2) + ' Tb';
  }
}
