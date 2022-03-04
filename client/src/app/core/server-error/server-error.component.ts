import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {
  error: any;
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    // see if we have the extras supplied from the error interceptor
    // this is some cool notation to simplify checking if something within an object exists
    // with this you dont have to have a seperate conditional for each field of the object
    // essentially equates to this.error = navigation && navigation.extras && navigation.extras.state
    // known as optional chaining
    this.error = navigation?.extras?.state?.error
  }

  ngOnInit(): void {
  }

}
