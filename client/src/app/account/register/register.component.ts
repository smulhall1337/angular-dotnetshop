import {Component, OnInit} from '@angular/core';
import {AsyncValidator, AsyncValidatorFn, FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../account.service";
import {Router} from "@angular/router";
import {of, timer} from "rxjs";

import {map, switchMap} from "rxjs/operators";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) {
  }

  ngOnInit(): void {
    this.CreateRegisterForm();
  }

  CreateRegisterForm() {
    this.registerForm = this.fb.group({
      displayName: [null, [Validators.required]],
      email: [
        null,
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
        [this.ValidateEmailNotTaken()]
      ],
      password: [null, Validators.required]
    });
  }

  OnSubmit() {
    this.accountService.Register(this.registerForm.value).subscribe(() => {
      this.router.navigateByUrl('/shop');
    }, error => {
      console.log(error)
      this.errors = error.errors;
    });
  }

  ValidateEmailNotTaken(): AsyncValidatorFn {
    // used for checkign whether an email exists as the user is typing
    return control => {
      // add a delay so the component doesnt send a request for every time the user types a character
      return timer(500).pipe(
        switchMap(() => {
          // we're returning an observable within an observable. we can use thw switchMap operator to help us out
          if (!control.value) {
            return of(null);
          }
          return this.accountService.CheckEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;
            })
          );
        }));
    };
  }
}
