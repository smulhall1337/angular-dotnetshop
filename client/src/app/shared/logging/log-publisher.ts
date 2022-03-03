import { observable, Observable, throwError } from 'rxjs';
import { of } from 'rxjs';
import 'rxjs/add/observable/of';
import { LogEntry } from './logging.service';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

export abstract class LogPublisher {
    location: string | undefined;
    abstract log(record: LogEntry):
        Observable<boolean>
    abstract clear(): Observable<boolean>;
}

export class LogConsole extends LogPublisher {
    log(record: LogEntry): Observable<boolean> {
        console.log(record.buildLogString());
        return of(true);
    }
    clear(): Observable<boolean> {
        console.clear();
        return of(true);
    }
}

export class LogLocalStorage extends LogPublisher {
    constructor() {
        // Must call `super()`from derived classes
        super();

        // Set location
        this.location = "logging";
    }

    // Append log entry to local storage
    log(entry: LogEntry): Observable<boolean> {
        let ret: boolean = false;
        let values: LogEntry[];

        try {
            if (this.location === undefined) {
                // gotta perform a null check because TS transpiler isnt smart enough to know 
                // that this.location is set in the constructor
                // ...kinda makes me miss plain old JS
                console.log("LogLocalStorage: Location cannot be undefined!");
            }
            else {
                if (localStorage === undefined) {

                }
                else {
                    // Get previous values from local storage
                    // adding an exclemation point here is called a non-null-assertion operator 
                    // basically, we're promising the TS transpiler that this value will not be null when
                    // execution reaches this point. see: https://www.typescriptlang.org/docs/handbook/release-notes/typescript-2-0.html
                    // 
                    // i could have done it on both getItem() and this.location, but i wanted to explicitly show what i was checking
                    // and i couldn't figure out how to satify the check for getItem().
                    values = JSON.parse(localStorage.getItem(this.location)!) || [];
                    // plus, im not a huge fan of notation like this, it falls under what I call "magic"
                    // where stuff is going on and its not really explicitly stated whats happenning under the hood.
                    // so I'm going to limit how many times i use stuff like this in the code.
                    // ok i'll shut up now

                    // Add new log entry to array
                    values.push(entry);

                    // Store array into local storage
                    localStorage.setItem(this.location, JSON.stringify(values));

                    // Set return value
                    ret = true;
                }
            }
        } catch (ex) {
            // Display error in console
            console.log(ex);
        }

        return Observable.of(ret);
    }

    // Clear all log entries from local storage
    clear(): Observable<boolean> {
        if (this.location === undefined) {
            console.log("LogLocalStorage: Location cannot be undefined!");
            return Observable.of(false);
        } else {
            localStorage.removeItem(this.location);
            return Observable.of(true);
        }
    }
}

export class LogWebApi extends LogPublisher {
    constructor(private http: HttpClient) {
        // Must call `super()`from derived classes
        super();
        
        // Set location
        this.location = "/api/log";
    }
    
    // Add log entry to back end data store
    log(entry: LogEntry): Observable<boolean> {
        let headers = new HttpHeaders().append( 'Content-Type', 'application/json' );
        let options = { headers: headers };
        
        // see comment in LogLocalStorage() about '!' operator
        return this.http.post(this.location!, entry, options).catch(this.handleErrors);
    }
    
    clear(): Observable<boolean> {
        // TODO: Call Web API to clear all values
        return Observable.of(true);
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
        return throwError(errors);
    }
}