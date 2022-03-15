import { Component, OnInit } from '@angular/core';
import {IProduct} from "./shared/models/product";
import {BasketService} from "./basket/basket.service";
import {AccountService} from "./account/account.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showModeratorBoard = false;
  title = 'Snap';
  products: IProduct[];

  constructor(private basketService: BasketService, private accountService: AccountService) { }

  LoadCurrentUser(){
    const token = localStorage.getItem('token');
    if (token) {
      this.accountService.LoadCurrentUser(token).subscribe(() => {
        console.log('loaded user');
      }, error => {
        console.log(error);
      })
    }
  }

  ngOnInit(): void {
    this.LoadCurrentUser();
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
