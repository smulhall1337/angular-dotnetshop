import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable, throwError} from "rxjs";
import {Router} from "@angular/router";
import {Injectable} from "@angular/core";
import {catchError} from "rxjs/operators";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {
  }


  /*
    Intercepts any errors that arise in the response coming back from the API
   */
  intercept(req: HttpRequest<any>, response: HttpHandler): Observable<HttpEvent<any>> {
    return response.handle(req).pipe(
      catchError(error => {
        if (error){
          if (error.status === 404){
            this.router.navigateByUrl('/not-found');
          }
          if (error.status === 500){
            this.router.navigateByUrl('/server-error');
          }
          return throwError(error); // anything else gets thrown
        }
      })
    );
  }

}
