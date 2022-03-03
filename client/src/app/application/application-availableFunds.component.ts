import { Component } from "@angular/core";
import { Output, EventEmitter } from '@angular/core';
import { ScrollToService } from "../services/scroll-to.service";

@Component({
    selector: 'app-application-availableFunds',
    templateUrl: './application-availableFunds.component.html',
    styleUrls: ['./application.component.scss']
})
export class ApplicationAvailableFundsComponent {
    @Output() showIncome: EventEmitter<any> = new EventEmitter();
    @Output() showHousehold: EventEmitter<any> = new EventEmitter();
    @Output() finishedAvailableFunds: EventEmitter<any> = new EventEmitter();

    constructor(private scrollToService: ScrollToService) {
    }

    // #region Cash On Hand Table
    addingCashOnHand: boolean = false;
    editCashOnHandField: string | undefined;
    cashOnHandList: Array<any> = [
      { id: 1, name: 'Aurelia Vega', amount: 30.00 },
      { id: 2, name: 'Guerra Cortez', amount: 45.62 },
      { id: 3, name: 'Guadalupe House', amount: 1337.42 },
      { id: 4, name: 'Aurelia Vega', amount: 66666666 },
      { id: 5, name: 'Elisa Gallagher', amount: -200 },
    ];

    awaitingCashOnHandList: Array<any> = [
      { id: 6, name: 'George Vega', amount: 28 },
      { id: 7, name: 'Mike Low', amount: 22 },
      { id: 8, name: 'John Derp', amount: 36 },
      { id: 9, name: 'Anastasia John', amount: 21 },
      { id: 10, name: 'John Maklowicz', amount: 36 },
    ];

    updateCashOnHandList(id: number, property: string, event: any) {
      const editField = event.target.textContent;
      this.cashOnHandList[id][property] = editField;
    }

    removeCashOnHand(id: any) {
      this.awaitingCashOnHandList.push(this.cashOnHandList[id]);
      this.cashOnHandList.splice(id, 1);
    }

    addCashOnHand() {
        this.addingCashOnHand = true;
    //   if (this.awaitingCashOnHandList.length > 0) {
    //     const person = this.awaitingCashOnHandList[0];
    //     this.cashOnHandList.push(person);
    //     this.awaitingCashOnHandList.splice(0, 1);
    //   }
    }

    changeCashOnHandValue(id: number, property: string, event: any) {
      this.editCashOnHandField = event.target.textContent;
    }
    // #endregion

    // #region Savings Accounts table
    editSavingsAccountField: string | undefined;
    SavingsAccountList: Array<any> = [
      { id: 1, name: 'Aurelia Vega', bankName: 'Capital One', amount: 30.00 },
      { id: 2, name: 'Guerra Cortez',bankName: 'Bank Of America', amount: 45.62 },
    ];

    awaitingSavingsAccountList: Array<any> = [
        { id: 3, name: 'Aurelia Vega', bankName: 'TD', amount: 30.00 },
        { id: 4, name: 'Guerra Cortez',bankName: 'Bank Of America', amount: 45.62 },
    ];

    updateSavingsAccountList(id: number, property: string, event: any) {
      const editField = event.target.textContent;
      this.cashOnHandList[id][property] = editField;
    }

    removeSavingsAccount(id: any) {
      this.awaitingCashOnHandList.push(this.cashOnHandList[id]);
      this.cashOnHandList.splice(id, 1);
    }

    addSavingsAccount() {
      if (this.awaitingCashOnHandList.length > 0) {
        const person = this.awaitingCashOnHandList[0];
        this.cashOnHandList.push(person);
        this.awaitingCashOnHandList.splice(0, 1);
      }
    }

    changeSavingsAccount(id: number, property: string, event: any) {
      this.editSavingsAccountField = event.target.textContent;
    }

    // #endregion


    // #region show variables
    showAvailableFunds: boolean = true;
    showCashOnHand: boolean = false;
    showSavingsAccounts: boolean = false;
    showCheckingAccounts: boolean = false;
    showInvestments: boolean = false;
    showRetirementAccounts: boolean = false;
    showTrustFunds: boolean = false;
    showPrepaidBurial: boolean = false;
    showOtherLiquid: boolean = false;
    // #endregion

    // #region add variables

    // #endregion


    HideAll() {
        this.showCashOnHand = false;
        this.showAvailableFunds = false;
        this.showSavingsAccounts = false;
        this.showCheckingAccounts = false;
        this.showInvestments = false;
        this.showRetirementAccounts = false;
        this.showTrustFunds = false;
        this.showPrepaidBurial = false;
        this.showOtherLiquid = false;
    }
    ShowHousehold() {
        this.showHousehold.emit();
    }

    // #region Continue to...
    BackToAvailableFundsStart() {
        this.HideAll();
        this.showAvailableFunds = true;
    }
    ContinueToCashOnHand() {
        this.HideAll();
        this.showCashOnHand = true;
        this.scrollToService.scrollTo("cashOnHand");
    }
    ContinueToSavingsAccounts() {
        this.HideAll();
        this.showSavingsAccounts = true;
        this.scrollToService.scrollTo("savingsAccounts");
    }
    ContinueToCheckingAccounts() {
        this.HideAll();
        this.showCheckingAccounts = true;
        this.scrollToService.scrollTo("checkingAccounts");
    }
    ContinueToInvestments() {
        this.HideAll();
        this.showInvestments = true;
        this.scrollToService.scrollTo("investments");
    }
    ContinueToRetirementAccounts() {
        this.HideAll();
        this.showRetirementAccounts = true;
        this.scrollToService.scrollTo("retirementAccounts");
    }
    ContinueToTrustFunds() {
        this.HideAll();
        this.showTrustFunds = true;
        this.scrollToService.scrollTo("trustFunds");
    }
    ContinueToPrepaidBurial() {
        this.HideAll();
        this.showPrepaidBurial = true;
        this.scrollToService.scrollTo("prepaidBurial");
    }
    ContinueToOtherLiquid() {
        this.HideAll();
        this.showOtherLiquid = true;
        this.scrollToService.scrollTo("otherLiquid");
    }
    ShowIncome() {
        this.HideAll();
        this.finishedAvailableFunds.emit();
        this.showIncome.emit();
    }

    // #endregion


}