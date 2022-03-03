import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {IProduct} from "../shared/models/product";
import {ShopService} from "./shop.service";
import {IBrand} from "../shared/models/brand";
import {IType} from "../shared/models/productType";
import {ShopParams} from "../shared/models/shopParams";

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  // our seach property is a child of the shop component
  // we use ViewChild to access that field
  // static field specifies whether or not this is a static element in our component(not relying on any dynamic activity)
  @ViewChild('search', {static: true}) searchTerm: ElementRef;

  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams = new ShopParams()
  totalCount: number;
  sortOptions =[
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ];
  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.GetProducts();
    this.GetBrands();
    this.GetTypes();
  }

  GetProducts(){
    this.shopService.GetProducts(this.shopParams).subscribe(response => {
      this.products = response.data;
      this.shopParams.pageNumber = response.pageIndex;
      this.shopParams.pageSize = response.pageSize;
      this.totalCount = response.count;
    }, error =>{
      console.log(error)
    });
  }

  GetBrands(){
    this.shopService.GetBrands().subscribe(response => {
      // ... = spread operator, puts all objects in repose to the array
      this.brands = [{id: 0, name: 'All'}, ...response];
    }, error =>{
      console.log(error)
    });
  }

  GetTypes(){
    this.shopService.GetTypes().subscribe(response => {
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error =>{
      console.log(error)
    });
  }

  OnBrandSelection(brandId: number){
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.GetProducts();
  }

  OnTypeSelected(typeId: number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.GetProducts();
  }

  OnSortSelected(sort: string){
    this.shopParams.sort = sort;
    this.GetProducts();
  }

  OnPageChanged(event: any) {
    if (this.shopParams.pageNumber != event) {
      this.shopParams.pageNumber = event;
      this.GetProducts();
    }
  }

  OnSearch(){
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.GetProducts();
  }

  OnReset(){
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.GetProducts();
  }
}
