import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
  }

  Get404Error() {
    this.http.get(this.baseUrl + 'products/42').subscribe(response => {
      console.log(response)
    }, error => console.log(error));
  }

  Get500Error() {
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe(response => {
      console.log(response)
    }, error => console.log(error));
  }

  Get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(response => {
      console.log(response)
    }, error => console.log(error));
  }

  Get400ValidationError() {
    this.http.get(this.baseUrl + 'products/fortytwo').subscribe(response => {
      console.log(response)
    }, error => console.log(error));
  }
}
