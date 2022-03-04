import { Component, OnInit } from '@angular/core';
import {BreadcrumbService} from "xng-breadcrumb";
import {Observable} from "rxjs";

/**
 * NOTE: Observable vs Promise
 *  1. Observables emit multiple values over a period of time, promises are single values at a time
 *  2. Observables are lazy, theyh dont execute until we call subscribe(), promises are not
 *  3. Observables are cancellable, promises are not
 *  4. observables provide the map for forEach, filter, reduce, retry, and retryWhen operators
 *
 */


@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit {
  breadcrumb$: Observable<any[]>; // convention is that observable names end wth $

  constructor(private bcService: BreadcrumbService) { }

  // quick intro: http requests (which are observables) are automatically destroyed by angular when they complete
  // but what if we have an observable that isnt an HTTP request?
  ngOnInit(): void {
    // rule of thumb: if you subscribe to something, you should unsubscribe from it.
    // this observable is NOT from an HTTP, what happens when this is disposed?
    // could use toPromise to turn it into a normal JS promise with .then(), .catch() and .finally()
    // this.bcService.breadcrumbs$.toPromise(), but we're not gonna do that here
    this.breadcrumb$ = this.bcService.breadcrumbs$;
  }

}
