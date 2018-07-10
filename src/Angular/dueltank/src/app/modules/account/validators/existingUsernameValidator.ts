import {AuthenticationService} from "../../../shared/services/authentication.service";
import { AsyncValidatorFn, AbstractControl, ValidationErrors} from "@angular/forms";
import {Observable} from "rxjs/Observable";


export function existingUsernameValidator(auth: AuthenticationService) : AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    return auth.checkUsernameNotTaken(control.value).map(
      resp => {
        return typeof(resp) == typeof(true) ? null : {"usernameExists": true};
      }
    );
  }
}
