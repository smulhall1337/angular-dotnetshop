import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ApplicationComponent } from './application/application.component';
import { ErrorComponent } from './error/error.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ScrollSpyDirective } from './scroll-spy.directive';
import { RouterModule } from '@angular/router';
import { ApplicationCardComponent } from './application-card/application-card.component';
import { LogPublishersService } from './shared/logging/log-publishers.service';
import { LoggingService } from './shared/logging/logging.service';
import { LogTestComponent } from './log-test/log-test.component';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import { RegisterComponent } from './register/register.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

import { authInterceptorProvider } from './helpers/auth.interceptor';
import { MaterialModule } from './material.module';
import { RegistrationCompleteComponent } from './registration-complete/registration-complete.component';
import { ChangePasswordComponent } from './user-profile/change-password.component';
import { LearnMoreComponent } from './home/learn-more.component';
import { GetAssistanceComponent } from './home/get-assistance.component';
import { SigninOrCreateAccountComponent } from './home/signin-or-create-account.component';
import { ApplicationCompleteComponent } from './application/application-complete.component';
import { LoginLandingComponent } from './login-landing/login-landing.component';
import { NgxNumberSpinnerModule } from 'ngx-number-spinner';
import { ApplicationPreambleComponent } from './application/application-preamble.component';
import { ApplicationHouseholdComponent } from './application/application-household.component';
import { ApplicationAvailableFundsComponent } from './application/application-availableFunds.component';
import { ApplicationIncomeComponent } from './application/application-income.component';
import { ApplicationExpensesComponent } from './application/application-expenses.component';
import { ApplicationSummaryComponent } from './application/application-summary.component';
import { NgForm, ReactiveFormsModule } from '@angular/forms';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import {CoreModule} from "./core/core.module";
import {ShopModule} from "./shop/shop.module";
import {ShopHomeComponent} from "./shop-home/shop-home.component";
import {ShopHomeModule} from "./shop-home/shop-home.module";


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ApplicationComponent,
    ErrorComponent,
    ScrollSpyDirective,
    ApplicationCardComponent,
    LogTestComponent,
    RegisterComponent,
    UserProfileComponent,
    RegistrationCompleteComponent,
    ChangePasswordComponent,
    LearnMoreComponent,
    GetAssistanceComponent,
    SigninOrCreateAccountComponent,
    ApplicationPreambleComponent,
    ApplicationHouseholdComponent,
    SigninOrCreateAccountComponent,
    ApplicationCompleteComponent,
    LoginLandingComponent,
    ApplicationAvailableFundsComponent,
    ApplicationIncomeComponent,
    ApplicationExpensesComponent,
    ApplicationSummaryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    NgxNumberSpinnerModule,
    MDBBootstrapModule,
    ReactiveFormsModule,
    CoreModule,
    ShopModule,
  ],
  providers: [
    LoggingService,
    LogPublishersService,
    authInterceptorProvider,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
