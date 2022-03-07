import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject} from "rxjs";
import {Basket, IBasket, IBasketItem} from "../shared/models/basket";
import {map} from "rxjs/operators";
import {IProduct} from "../shared/models/product";

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null); //always going to emit an initial value.
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) {
  }

  GetBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          console.log(this.GetCurrentBasketValue());
        })
      );
  }

  SetBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket)
      .subscribe((response: IBasket) => {
          this.basketSource.next(response);
          console.log(response);
        }
        , error => {
          console.log(error)
        });
  }

  GetCurrentBasketValue() {
    return this.basketSource.value;
  }

  AddItemToBasket(item: IProduct, quanity = 1) {
    // need a mapper to map product item to basket item
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quanity);
    const basket = this.GetCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quanity);
    this.SetBasket(basket);
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      brand: item.productBrand,
      pictureUrl: item.pictureUrl,
      price: item.price,
      productName: item.name,
      quantity: quantity,
      type: item.productType
    };
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quanity: number) {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quanity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quanity;
    }
    return items;
  }
}
