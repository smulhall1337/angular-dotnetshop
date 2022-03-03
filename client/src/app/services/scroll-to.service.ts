import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ScrollToService {

  constructor() { }

  scrollTo(section: string) {
    setTimeout(() => {
      document.querySelector('#' + section)!
        .scrollIntoView({ behavior: "smooth" });

      // HACK: we cant use the scrollTo() function immediately when clicking "Next" since JS is asynchronus and it tries to scroll before
      // the DOM has been rendered. A dirty workaround is to make the browser wait before executing scrollTo() to ensure the 
      // necessary DOM has been loaded... Theres probably a better way to do this.
      // 
      // TODO: Find a better way to do this
    }, 50); // <- milliseconds
  }
}
