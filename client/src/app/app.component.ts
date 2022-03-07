import { Component, OnInit } from '@angular/core';
import {IProduct} from "./shared/models/product";
import {BasketService} from "./basket/basket.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showModeratorBoard = false;
  title = 'Snap';
  products: IProduct[];

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    const basketId = localStorage.getItem('basket_id'); // see if theres an existing basket
    if (basketId)
    {
      this.basketService.GetBasket(basketId).subscribe(() => {
        console.log('basket initialised from local storage')
      }, error => {
        console.log(error);
        }
      );
    }
  }
}
