import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AccountService} from "../account.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrl: string;

  constructor(private accountService: AccountService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop'
    this.CreateLoginForm();
  }

  CreateLoginForm(){
    this.loginForm = new FormGroup({
      email: new FormControl('', [
          Validators.required,
        Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]
      ),
      password: new FormControl('', Validators.required)
    })
  }

  OnSubmit(){
    this.accountService.Login(this.loginForm.value).subscribe(() => {
      this.router.navigateByUrl(this.returnUrl); // send the user to where they were trying to go to before they logged in (if anything)
      }, error => {
      console.log(error)
    });
  }

}
