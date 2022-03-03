import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {IPagination} from "../shared/models/pagination";
import {IBrand} from "../shared/models/brand";
import {IType} from "../shared/models/productType";
import {map} from "rxjs/operators";
import {ShopParams} from "../shared/models/shopParams";
import {IProduct} from "../shared/models/product";

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:7229/api/v1/';
  constructor(private http: HttpClient) {}

  GetProducts(shopParams: ShopParams){
    // get usually returns a generic Object type which could be anything
    // we can specify what type we're expecting with <Type>
    let params = new HttpParams();
    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }
    if(shopParams.search){
      params = params.append('search', shopParams.search);
    }
    params = params.append('sort', shopParams.sort.toString());
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageIndex', shopParams.pageSize.toString());


    // we're 'observing' the response that will return an http response and not the body of the response
    // we'll need to extract the body of the response to be able to use it
    // we'll have to map the http response to an Ipagination
    return this.http.get<IPagination>(this.baseUrl+"products", {observe: 'response', params})
      .pipe(
        map(response => {
          return response.body;
        })
      );
  }

  GetBrands(){
    return this.http.get<IBrand[]>(this.baseUrl+'products/brands');
  }

  GetTypes(){
    return this.http.get<IType[]>(this.baseUrl+'products/types');
  }

  GetProduct(id: number)
  {
    return this.http.get<IProduct>(this.baseUrl+'products/'+id);
  }

}
