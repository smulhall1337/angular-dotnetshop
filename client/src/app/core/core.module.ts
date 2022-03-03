import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {StoreNavBarComponent} from "./store-nav-bar/store-nav-bar.component";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [StoreNavBarComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    StoreNavBarComponent
  ]
})
export class CoreModule { }
