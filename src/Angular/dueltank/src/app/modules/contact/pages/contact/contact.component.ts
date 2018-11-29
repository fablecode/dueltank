import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";

@Component({
  templateUrl: './contact.component.html'
})
export class ContactComponent implements OnInit {
  private contactForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.createFormControls();
    this.createForm();
  }

  private createForm() : void {
    this.contactForm = this.fb.group([])
  }

  private createFormControls() : void {

  }

  public onSubmit() : void {

  }
}
