import {Injectable} from '@angular/core';
import {Contact} from "../../../shared/models/contact/contact";
import {Observable} from "rxjs";
import {ContactResult} from "../../../shared/models/contact/contact-result";
import {HttpClient} from "@angular/common/http";
import {AppConfigService} from "../../../shared/services/app-config.service";

@Injectable()
export class ContactService {

  constructor(private http: HttpClient, private configuration: AppConfigService) { }

  public sendMessage(contact: Contact) : Observable<ContactResult> {
    return this.http.post<ContactResult>(this.configuration.apiEndpoint + "/api/contactmessages/", contact)
  }
}
