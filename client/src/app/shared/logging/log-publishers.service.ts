import { Injectable } from '@angular/core';
import { LogPublisher, LogConsole, LogLocalStorage, LogWebApi } from "./log-publisher";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LogPublishersService {

  // TODO: read publishers from json file
  constructor(private httpClient: HttpClient) {
    // Build publishers arrays
    this.buildPublishers();
  }

  // Public properties
  publishers: LogPublisher[] = [];

  // Build publishers array
  buildPublishers(): void {
    // Create instance of LogConsole Class
    this.publishers.push(new LogConsole());

    // Create instance of `LogLocalStorage` Class
    this.publishers.push(new LogLocalStorage());

    // Create instance of `LogWebApi` Class
    this.publishers.push(new LogWebApi(this.httpClient));
  }

  private handleErrors(error: any): Observable<any> {
    let errors: string[] = [];
    let msg: string = "";

    msg = "Status: " + error.status;
    msg += " - Status Text: " + error.statusText;
    if (error.json()) {
      msg += " - Exception Message: " + error.json().exceptionMessage;
    }
    errors.push(msg);
    console.error('An error occurred', errors);
    return Observable.throw(errors);
  }
}
