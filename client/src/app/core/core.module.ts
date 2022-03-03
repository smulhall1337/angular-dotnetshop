import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {StoreNavBarComponent} from "./store-nav-bar/store-nav-bar.component";
import {RouterModule} from "@angular/router";
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';



@NgModule({
  declarations: [StoreNavBarComponent, TestErrorComponent, NotFoundComponent, ServerErrorComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    StoreNavBarComponent
  ]
})
export class CoreModule { }
