import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import { Observable } from 'rxjs';
import {AccountService} from "../../account/account.service";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private router: Router) {
  }
  // Check whether a user can access a specified resource
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.accountService.currentUser$.pipe(

      map(auth => {
        if (auth){
          return true; // currently logged in
        }
        // if they're not, send them to login
        // once they're logged in, we can specify a return url where they are sent once they log in
        this.router.navigate(['account/login'], {queryParams:{returnUrl: state.url}})
      })
    );
  }
}
