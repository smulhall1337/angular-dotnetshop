import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {BusyService} from "../services/busy.service";
import {Injectable} from "@angular/core";
import {delay, finalize} from "rxjs/operators";

/**
 * provides an app-wide loading screen
 */

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // increment busy count
    this.busyService.busy();
    return next.handle(req).pipe(
      delay(1000),
      finalize(() => {
        // whenever we're done with this, decrement the busy count
        this.busyService.idle();
      })
    )
  }

}
