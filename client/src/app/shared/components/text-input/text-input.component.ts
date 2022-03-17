import {Component, ElementRef, Input, OnInit, Self, ViewChild} from '@angular/core';
import {ControlValueAccessor, NgControl} from "@angular/forms";

/**
 * component for working with form controls. detects when a form control is changed on the DOM
 */
@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, ControlValueAccessor{

  @ViewChild('input', {static: true}) input: ElementRef;
  @Input() type = 'text';
  @Input() label: string;

  // in order to access validation, we need to access the control itself
  // to do that, we'll inject the control here
  constructor(@Self() public controlDir: NgControl) {
    // NgControl is what our form control derives from
    // @Self decorator tells angular to only look inside itself and not another shared dependency
    this.controlDir.valueAccessor = this;
    // binds 'this' to our class and gives us access to the controldir within our component and in our template
  }

  // these methods will be defined below in their respective register method
  OnChange(event){}
  OnTouched(){}

  ngOnInit(): void {
    const control = this.controlDir.control;
    const validators = control.validator ? [control.validator]: [];
    const asyncValidators = control.asyncValidator ? [control.asyncValidator]: [];

    control.setValidators(validators);
    control.setAsyncValidators(asyncValidators);
    control.updateValueAndValidity();
  }

  registerOnChange(fn: any): void {
    this.OnChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.OnTouched = fn;
  }

  writeValue(obj: any): void {
    // gives our controlvalueaccessor access to the values that are entered into our input field
    this.input.nativeElement.value = obj || '';
  }
}
