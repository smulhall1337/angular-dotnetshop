import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject} from "rxjs";
import {Basket, IBasket, IBasketItem, IBasketTotals} from "../shared/models/basket";
import {map} from "rxjs/operators";
import {IProduct} from "../shared/models/product";

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null); //always going to emit an initial value.
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) {
  }

  GetBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        })
      );
  }

  SetBasket(basket: IBasket) {
    return this.http.post(this.baseUrl + 'basket', basket)
      .subscribe((response: IBasket) => {
          this.basketSource.next(response);
          this.calculateTotals();
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

  IncrementItemQuantity(item: IBasketItem){
    const basket = this.GetCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id)
    basket.items[foundItemIndex].quantity++;
    this.SetBasket(basket);
  }

  DecrementItemQuantity(item: IBasketItem){
    const basket = this.GetCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id)
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this.SetBasket(basket);
    } else {
      this.RemoveItemFromBasket(item);
    }
  }

  RemoveItemFromBasket(item: IBasketItem){
    const basket = this.GetCurrentBasketValue();
    // .some checks to see if at least one item in the collection matches the rule we set in the callback
    if (basket.items.some(x => x.id === item.id)){
      // .filter returns the items in the collection minus the one we want to remove
      basket.items = basket.items.filter(i => i.id !== item.id);
    }
    if (basket.items.length >0){
      this.SetBasket(basket);
    } else {
      this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: IBasket){
    return this.http.delete(this.baseUrl+'basket?id='+basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket)id');
    }, error => {
      console.log(error);
    });
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

  private calculateTotals(){
    const basket = this.GetCurrentBasketValue();
    const shipping = 0;
    // reduce applies the callback function we provide to every element of the array
    const subtotal = basket.items.reduce((result, item) => (item.price * item.quantity) + result, 0); // 0 = result initial value
    const total = shipping + subtotal;
    this.basketTotalSource.next({shipping, total, subtotal})
  }
}
