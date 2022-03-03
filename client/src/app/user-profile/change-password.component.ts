import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../services/token-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  currentUser: any;

  constructor(private token: TokenStorageService, private router: Router) { }

  ngOnInit(): void {
    this.currentUser = this.token.getUser();
  }

  ChangePassword(){
    this.router.navigate(['/profile']);
  }
}