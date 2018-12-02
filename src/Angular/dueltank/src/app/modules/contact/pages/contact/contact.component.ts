import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {ContactService} from "../../services/contact.service";
import {Contact} from "../../../../shared/models/contact/contact";
import {ContactResult} from "../../../../shared/models/contact/contact-result";


@Component({
  templateUrl: './contact.component.html'
})
export class ContactComponent implements OnInit {
  public contactForm: FormGroup;
  public name: FormControl;
  public email: FormControl;
  public message: FormControl;

  constructor(private fb: FormBuilder, private contactService: ContactService) { }

  ngOnInit() {
    this.createFormControls();
    this.createForm();
  }

  private createForm() : void {
    this.contactForm = this.fb.group({
      name: this.name,
      email: this.email,
      message: this.message
    });
  }

  private createFormControls() : void {
    this.name = new FormControl('', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(255)
    ]);

    this.email = new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(254),
      Validators.email,
      Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")
    ]);

    this.message = new FormControl('', [
      Validators.required,
      Validators.minLength(4),
      Validators.maxLength(2083)
    ])
  }

  public onSubmit() : void {
    if(this.contactForm.valid) {
      let contact = new Contact();

      contact.name = this.contactForm.controls.name.value;
      contact.email = this.contactForm.controls.email.value;
      contact.message = this.contactForm.controls.message.value;

      this.contactService
        .sendMessage(contact)
        .subscribe((contactResult: ContactResult) => {
          console.log("Contact message sent!")
        });
    }
  }
}
