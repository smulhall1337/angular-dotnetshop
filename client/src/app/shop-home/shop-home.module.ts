import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopHomeComponent } from './shop-home.component';
import {SharedModule} from "../shared/shared.module";



@NgModule({
  declarations: [
    ShopHomeComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [ShopHomeComponent]
})
export class ShopHomeModule { }
