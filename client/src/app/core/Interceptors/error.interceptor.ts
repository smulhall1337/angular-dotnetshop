import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable, throwError} from "rxjs";
import {NavigationExtras, Router} from "@angular/router";
import {Injectable} from "@angular/core";
import {catchError, delay} from "rxjs/operators";
import {ToastrService} from "ngx-toastr";


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastrService: ToastrService) {
  }


  /*
    Intercepts any errors that arise in the response coming back from the API
   */
  intercept(req: HttpRequest<any>, response: HttpHandler): Observable<HttpEvent<any>> {
    return response.handle(req).pipe(
      catchError(error => {
        if (error){
          if (error.status === 400){
            if (error.error.errors){
              // validation errors return an object (called error) that contains an errors array
              // that contains the actual error returned from the server
              // if the response has the array, return to the component so the component can display it as an error message
              throw error.error;
            }
            else {
              this.toastrService.error(error.error.message, error.error.statusCode);
            }
          }
          if (error.status === 401){
            this.toastrService.error(error.error.message, error.error.statusCode);
          }
          if (error.status === 404){
            this.router.navigateByUrl('/not-found');
          }
          if (error.status === 500){
            // thanks to angular 7, we can pass states via NavigationExtras
            // we define a state, called error, which gets its content from the error within the error object
            // which is unfortunately also titled error... I know. believe me i know
            const navigationExtras: NavigationExtras = {state:{error: error.error}}
            this.router.navigateByUrl('/server-error', navigationExtras);
          }
          return throwError(error); // anything else gets thrown
        }
      })
    );
  }

}
