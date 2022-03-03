import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {ApplicationComponent} from './application/application.component';
import {LogTestComponent} from './log-test/log-test.component';
import {RegisterComponent} from './register/register.component';
import {RegistrationCompleteComponent} from './registration-complete/registration-complete.component';
import {UserProfileComponent} from './user-profile/user-profile.component';
import {ChangePasswordComponent} from './user-profile/change-password.component';
import {LearnMoreComponent} from './home/learn-more.component';
import {GetAssistanceComponent} from './home/get-assistance.component';
import {SigninOrCreateAccountComponent} from './home/signin-or-create-account.component';
import {ErrorComponent} from './error/error.component';
import {ApplicationCompleteComponent} from './application/application-complete.component';
import {LoginLandingComponent} from './login-landing/login-landing.component';
import {ShopHomeComponent} from "./shop-home/shop-home.component";
import {ShopComponent} from "./shop/shop.component";
import {ProductDetailsComponent} from "./shop/product-details/product-details.component";

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'home/learnmore',
    component: LearnMoreComponent
  },
  {
    path: 'home/getassistance',
    component: GetAssistanceComponent
  },
  {
    path: 'home/signingorcreateaccount',
    component: SigninOrCreateAccountComponent
  },
  {
    path: 'home/welcome',
    component: LoginLandingComponent
  },
  {
    path: 'apply',
    component: ApplicationComponent
  },
  {
    path: 'apply/complete',
    component: ApplicationCompleteComponent
  },
  {
    path: 'log',
    component: LogTestComponent
  },
  {
    path: 'registration',
    component: RegisterComponent
  },
  {
    path: 'registration/complete',
    component: RegistrationCompleteComponent
  },
  {
    path: 'profile',
    component: UserProfileComponent
  },
  {
    path: 'profile/changepassword',
    component: ChangePasswordComponent
  },
  // {
  //   path: '',
  //   component: HomeComponent
  // },
  // shop routes
  {
    path: '',
    component: ShopHomeComponent
  },
  {
    path: 'shop',
    // lazy loading of routes from the shop module
    // see /shop/shoproutingmodule
    loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule)
  },
  // end shop routes
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
  // {
  //   path: '**', // wildcard, matched nothing else
  //   component: ErrorComponent
  // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    anchorScrolling: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
