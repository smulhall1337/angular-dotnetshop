import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {BehaviorSubject} from "rxjs";
import {IUser} from "../shared/models/user";
import {map} from "rxjs/operators";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<IUser>(null);
  currentUser$ = this.currentUserSource.asObservable();


  constructor(private http: HttpClient, private router: Router) {}

  Login(values: any) {
    return this.http.post(this.baseUrl+'account/login', values).pipe(
      map((user: IUser) => {
        if(user){
          // persist the login
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  Register(values: any){
    return this.http.post(this.baseUrl+'account/register', values).pipe(
      map((user: IUser) => {
        if(user){
          // persist the login
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  Logout(){
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  CheckEmailExists(email: string){
    return this.http.get(this.baseUrl+'account/emailexists?email='+email);
  }
}
