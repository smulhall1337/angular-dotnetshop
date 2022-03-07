import { Component, OnInit } from '@angular/core';
import {BasketService} from "../../basket/basket.service";
import {Observable} from "rxjs";
import {IBasket} from "../../shared/models/basket";

@Component({
  selector: 'app-store-nav-bar',
  templateUrl: './store-nav-bar.component.html',
  styleUrls: ['./store-nav-bar.component.scss']
})
export class StoreNavBarComponent implements OnInit {
  basket$: Observable<IBasket>;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

}
