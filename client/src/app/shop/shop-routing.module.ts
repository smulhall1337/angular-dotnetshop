import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {ShopComponent} from "./shop.component";
import {ProductDetailsComponent} from "./product-details/product-details.component";

const routes: Routes = [
  // lazy loading of routes. only load the components when they are accessed
  {
    // dont need a path because this is the root component of the shop module
    path: '',
    component: ShopComponent
  },
  {
    path: ':id',
    component: ProductDetailsComponent
  },
]

@NgModule({
  declarations: [],
  imports: [
    // forChild because these routes are only going to be available for the shop module
    // not the root app module.
    RouterModule.forChild(routes),
    CommonModule
  ],
  exports: [
    RouterModule
  ]
})
export class ShopRoutingModule { }
