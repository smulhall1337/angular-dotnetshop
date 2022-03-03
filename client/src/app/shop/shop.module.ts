import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import {SharedModule} from "../shared/shared.module";
import { ProductDetailsComponent } from './product-details/product-details.component';
import {ShopRoutingModule} from "./shop-routing.module";



@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductDetailsComponent,
  ],
  // we dont need to export the shop module since lazy loading is taking
  // care of that for us
    imports: [
        CommonModule,
        SharedModule,
        ShopRoutingModule
    ]
})
export class ShopModule { }
