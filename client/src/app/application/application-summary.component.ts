import { ViewportScroller } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ScrollToService } from '../services/scroll-to.service';

@Component({
    selector: 'app-application-summary',
    templateUrl: './application-summary.component.html',
    styleUrls: ['./application.component.scss'],
})
export class ApplicationSummaryComponent {
    constructor(private scrollToService: ScrollToService) { }

    @Output() showExpenses: EventEmitter<any> = new EventEmitter();

    showSummary: boolean = true;


    showLifeline: boolean = false;
    editingLifeLine: boolean = false;
    doneLifeLine: boolean = false;

    showLocation: boolean = false;
    editingLocation: boolean = false;
    doneLocation: boolean = false;

    showStatementOfUnderstanding: boolean = false;
    editingStatementOfUnderstanding: boolean = false;
    doneStatementOfUnderstanding: boolean = false;

    showFinalSummary: boolean = false;


    HideAll(){
        this.showSummary = false;
        this.showLifeline = false;
        this.showLocation = false;
        this.showStatementOfUnderstanding = false;
        this.showFinalSummary = false;
    }

    ShowSummary(){
        this.HideAll();
        this.showSummary = true;
        this.scrollToService.scrollTo("summary");
    }

    ShowExpenses() {
        this.showExpenses.emit();
    }

    ContinueToLifeline() {
        this.HideAll();
        this.showLifeline = true;
        this.scrollToService.scrollTo("lifeLine");
    }

    EditLifeLine() {
        this.editingLifeLine = true;
        this.doneLifeLine = false;
    }
    SaveLifeLine() {
        this.editingLifeLine = false;
        this.doneLifeLine = true;
    }
    ContinueToLocation() {
        this.HideAll();
        this.showLocation = true;
        this.doneLifeLine = true;
        this.scrollToService.scrollTo("location");
    }

    EditLocation() {
        this.editingLocation = true;
        this.doneLocation = false;
    }
    SaveLocation() {
        this.editingLocation = false;
        this.doneLocation = true;
    }
    ContinueToStatementOfUnderstanding() {
        this.HideAll();
        this.showStatementOfUnderstanding = true;
        this.doneLocation = true;
        this.scrollToService.scrollTo("statementOfUnderstanding");
    }

    ContinueToFinalSummary() {
        this.HideAll();
        this.showFinalSummary = true;
        this.doneStatementOfUnderstanding = true;
        this.scrollToService.scrollTo("finalSummary");
    }

}