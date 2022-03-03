import { ViewportScroller } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ScrollToService } from '../services/scroll-to.service';

@Component({
    selector: 'app-application-income',
    templateUrl: './application-income.component.html',
    styleUrls: ['./application.component.scss'],
})
export class ApplicationIncomeComponent{
    @Output() showAvailableFunds: EventEmitter<any> = new EventEmitter();
    @Output() showExpenses: EventEmitter<any> = new EventEmitter();
    @Output() finishedIncome: EventEmitter<any> = new EventEmitter();

    constructor(private scrollToService: ScrollToService) {
    }

    showIncome: boolean = true;
    showJobIncome: boolean = false;
    editingJobIncome: boolean = false;
    doneJobIncome: boolean = false;

    showSelfEmploymentIncome: boolean = false;
    editingSelfEmploymentIncome: boolean = false;
    doneSelfemploymentIncome: boolean = false;

    showRoomAndBoard: boolean = false;
    editingRoomAndBoard: boolean = false;
    doneRoomAndBoard: boolean = false;

    showUnearnedIncome: boolean = false;
    editingUnearnedIncome: boolean = false;
    doneUnearnedIncome: boolean = false;

    ShowAvailableFunds() {
        this.showAvailableFunds.emit();
    }

    HideAll() {
        this.showIncome = false;
        this.showJobIncome = false;
        this.showRoomAndBoard = false;
        this.showSelfEmploymentIncome = false;
        this.showUnearnedIncome = false;
    }


    ShowIncome() {
        this.HideAll();
        this.showIncome = true;
        this.scrollToService.scrollTo("income");
    }
    ContinueToJobIncome() {
        this.HideAll();
        this.showJobIncome = true;
        this.scrollToService.scrollTo("jobIncome")
    }

    EditJobIncome() {
        this.editingJobIncome = true;
        this.doneJobIncome = false;
    }
    SaveJobIncome() {
        this.doneJobIncome = true;
        this.editingJobIncome = false;
    }
    ContinueToSelfEmploymentIncome() {
        this.HideAll();
        this.showSelfEmploymentIncome = true;
        this.scrollToService.scrollTo("selfEmploymentIncome")
    }

    EditSelfEmploymentIncome() {
        this.doneSelfemploymentIncome = false;
        this.editingSelfEmploymentIncome = true;
    }
    SaveSelfEmploymentIncome() {
        this.editingSelfEmploymentIncome = false;
        this.doneSelfemploymentIncome = true;
    }
    ContinueToRoomAndBoard() {
        this.HideAll();
        this.showRoomAndBoard = true;
        this.scrollToService.scrollTo("roomAndBoardIncome")
    }

    EditRoomAndBoard() {
        this.editingRoomAndBoard = true;
        this.doneRoomAndBoard = false;
    }
    SaveRoomAndBoard() {
        this.editingRoomAndBoard = false;
        this.doneRoomAndBoard = true;
    }
    ContinueToUnearnedIncome() {
        this.HideAll();
        this.showUnearnedIncome = true;
        this.scrollToService.scrollTo("unearnedIncome");
    }

    EditUnearnedIncome() {
        this.editingUnearnedIncome = true;
        this.doneUnearnedIncome = false;
    }
    SaveUnearnedIncome() {
        this.doneUnearnedIncome = true;
        this.editingUnearnedIncome = false;
    }

    FinishIncome() {
        this.finishedIncome.emit();
    }
    ContinueToExpenses() {
        this.HideAll();
        this.FinishIncome();
        this.showExpenses.emit();
    }

}
