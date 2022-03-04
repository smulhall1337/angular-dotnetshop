import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {StoreNavBarComponent} from "./store-nav-bar/store-nav-bar.component";
import {RouterModule} from "@angular/router";
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import {ToastrModule} from "ngx-toastr";
import { SectionHeaderComponent } from './section-header/section-header.component';
import {BreadcrumbModule} from "xng-breadcrumb";



@NgModule({
  declarations: [StoreNavBarComponent, TestErrorComponent, NotFoundComponent, ServerErrorComponent, SectionHeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    BreadcrumbModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right', // location of window
      preventDuplicates: true
    })
  ],
  exports: [
    StoreNavBarComponent,
    SectionHeaderComponent
  ]
})
export class CoreModule { }
