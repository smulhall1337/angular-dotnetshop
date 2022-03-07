import {Component, OnInit} from '@angular/core';
import {IProduct} from "../../shared/models/product";
import {ShopService} from "../shop.service";
import {ActivatedRoute} from "@angular/router";
import {BreadcrumbService} from "xng-breadcrumb";
import {BasketService} from "../../basket/basket.service";

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  product: IProduct;
  quantity = 1;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
              private bcService: BreadcrumbService, private basketService: BasketService) {
    // set the breadcrumb header to empty until the page and product loads
    this.bcService.set('@productDetails', '');
  }

  ngOnInit(): void {
    this.LoadProduct();
  }

  AddItemToBasket(){
    this.basketService.AddItemToBasket(this.product, this.quantity);
  }

  Increment(){
    this.quantity++;
  }
  Decrement(){
    if (this.quantity > 1){
      this.quantity--;
    }
  }

  LoadProduct() {
    // activatedRoute can pull the id out from the current path
    // the + is a cast to int
    this.shopService.GetProduct(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(product => {
        this.product = product
        // substitute the defined alias (in product routing module) with the products name
        this.bcService.set('@productDetails', product.name);
      },
      error => {
        console.log(error)
      });
  }

}
