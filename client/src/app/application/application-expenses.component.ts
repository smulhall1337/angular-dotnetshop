import { ViewportScroller } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ScrollToService } from '../services/scroll-to.service';

@Component({
    selector: 'app-application-expenses',
    templateUrl: './application-expenses.component.html',
    styleUrls: ['./application.component.scss'],
})
export class ApplicationExpensesComponent {
    constructor(private scrollToService: ScrollToService) { }

    @Output() showIncome: EventEmitter<any> = new EventEmitter();
    @Output() showSummary: EventEmitter<any> = new EventEmitter();
    @Output() finishedExpenses: EventEmitter<any> = new EventEmitter();


    showExpenses: boolean = true;

    showHouseholdDescription: boolean = false;
    editingHouseholdDescription: boolean = false;
    doneHouseholdDescription: boolean = false;

    showUtilities = false;
    editingUtilities = false;
    doneUtilities = false;

    showDependentCare = false;
    editingDependentCare = false;
    doneDependentCare = false;

    showMedicalExpenses = false;
    editingMedicalExpenses = false;
    doneMedicalExpenses = true;


    HideAll() {
        this.showExpenses = false;
        this.showHouseholdDescription = false;
        this.showUtilities = false;
        this.showDependentCare = false;
        this.showMedicalExpenses = false;
    }

    ShowIncome() {
        this.showIncome.emit();
    }

    ShowExpenses() {
        this.HideAll();
        this.showExpenses = true;
    }

    ContinueToHouseholdDescription() {
        this.HideAll();
        this.showHouseholdDescription = true;
        this.scrollToService.scrollTo("householdDescription");
    }

    EditHouseholdDescription() {
        this.editingHouseholdDescription = true;
        this.doneHouseholdDescription = false;
    }
    SaveHouseholdDescription() {
        this.editingHouseholdDescription = false;
        this.doneHouseholdDescription = true;
    }
    ContinueToUtilities() {
        this.HideAll();
        this.showUtilities = true;
        this.doneHouseholdDescription = true;
        this.scrollToService.scrollTo("utilities");
    }

    EditUtilities() {
        this.doneUtilities = false;
        this.editingUtilities = true;
    }
    SaveUtilities() {
        this.doneUtilities = true;
        this.editingUtilities = false;
    }
    ContinueToDependentCare() {
        this.HideAll();
        this.doneUtilities = true;
        this.showDependentCare = true;
        this.scrollToService.scrollTo("dependentCare");
    }

    EditDependentCare() {
        this.doneDependentCare = false;
        this.editingDependentCare = true;
    }
    SaveDependentCare() {
        this.doneDependentCare = true;
        this.editingDependentCare = false;
    }
    ContinueToMedicalExpenses() {
        this.HideAll();
        this.showMedicalExpenses = true;
        this.doneDependentCare = true;
        this.scrollToService.scrollTo("medicalExpenses");
    }

    EditMedicalExpenses() {
        this.doneMedicalExpenses = false;
        this.editingMedicalExpenses = true;
    }
    SaveMedicalExpenses() {
        this.editingMedicalExpenses = false;
        this.doneMedicalExpenses = true;
    }

    FinishExpenses(){
        this.finishedExpenses.emit();
    }
    ContinueToSummary() {
        this.HideAll();
        this.FinishExpenses();
        this.showSummary.emit();
    }


    // #endregion

}
