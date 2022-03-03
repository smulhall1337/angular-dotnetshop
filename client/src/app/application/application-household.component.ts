import { Component } from "@angular/core";
import { Output, EventEmitter } from '@angular/core';
import { ScrollToService } from "../services/scroll-to.service";


@Component({
    selector: 'app-application-household',
    templateUrl: './application-household.component.html',
    styleUrls: ['./application.component.scss']
})
export class ApplicationHouseholdComponent {
    @Output() showAvailableFunds: EventEmitter<any> = new EventEmitter();
    @Output() showPreamble: EventEmitter<any> = new EventEmitter();
    @Output() finishedHousehold: EventEmitter<any> = new EventEmitter();

    constructor(private scrollToService: ScrollToService) {

    }

    showHousehold: boolean = true;
    editingHousehold: boolean = false;
    // finishedHousehold: boolean = false;

    showApplicant: boolean = false;
    doneApplicant: boolean = false;
    editingApplicant: boolean = false;

    showContact: boolean = false;
    doneContact: boolean = false;
    editingContact: boolean = false;

    showHouseholdQuestionnaire: boolean = false;
    editingQuestionnaire: boolean = false;
    doneQuestionnaire: boolean = false;

    showHouseholdSize: boolean = false;
    doneHouseholdSize: boolean = false;
    editingHouseholdSize: boolean = false;

    showHeadOfHousehold: boolean = false;
    editingHeadOfHousehold: boolean = false;
    doneHeadOfHousehold: boolean = false;

    showTellUsMore: boolean = false;
    editingHouseholdMember: boolean = false;
    doneHouseholdMember: boolean = false;

    showAccountRep: boolean = false;
    doneAccountRep: boolean = false;
    editingAccountRep: boolean = false;

    finishedAccountRep: boolean = false;

    HideAll() {
        this.showHousehold = false;
        this.showApplicant = false;
        this.showContact = false;
        this.showHouseholdQuestionnaire = false;
        this.showHouseholdSize = false;
        this.showTellUsMore = false;
        this.showAccountRep = false;
        this.showHeadOfHousehold = false;
    }

    change(value: number): void {
        console.log(value);
      }


    BackToPreamble() {
        this.showPreamble.emit();
    }

    BackToHouseholdStart(){
        this.HideAll();
        this.showHousehold = true;
        this.scrollToService.scrollTo("applicant");
    }
    ContinueToApplicant() {
        this.HideAll()
        this.showApplicant = true;
        this.scrollToService.scrollTo("applicant");
    }
    editApplicant() {
        //this.doneHouseholdInfo = false;
        this.editingApplicant = true;
    }

    SaveApplicant() {
        this.scrollToService.scrollTo
    }
    ContinueToContact() {
        this.HideAll();
        this.showContact = true;
        this.scrollToService.scrollTo("contact");
    }

    EditContact() {
        this.doneContact = false;
        this.editingContact = true;
    }
    SaveContact() {
        this.editingContact = false;
        this.doneContact = true;
    }
    ContinueToQuestionnaire() {
        this.HideAll();
        this.showHouseholdQuestionnaire = true;
        this.scrollToService.scrollTo("questionnaire");
    }


    EditQuestionnaire() {
        this.editingQuestionnaire = true;
        this.doneQuestionnaire = false;
    }
    SaveQuestionnaire() {
        this.doneQuestionnaire = true;
        this.editingQuestionnaire = false;
    }
    ContinueToHouseholdSize() {
        this.HideAll();
        this.showHouseholdSize = true;
        this.scrollToService.scrollTo("householdSize");
    }

    
    EditHouseholdSize() {
        this.editingHouseholdSize = true;
        this.doneHouseholdSize = false;
    }
    SaveHouseholdSize() {
        this.doneHouseholdSize = true;
        this.editingHouseholdSize = false;
    }
    ContinueToHeadOfHousehold() {
        this.HideAll();
        this.showHeadOfHousehold = true;
        this.scrollToService.scrollTo("headOfHousehold");
    }


    EditHeadOfHousehold() {
        this.editingHeadOfHousehold = true;
        this.doneHeadOfHousehold = false;
    }
    SaveHeadOfHousehold() {
        this.doneHeadOfHousehold = true;
        this.editingHeadOfHousehold = false;
    }
    ContinueToTellUsMore() {
        this.HideAll();
        this.showTellUsMore = true;
        this.scrollToService.scrollTo("tellUsMore");
    }


    EditHouseholdMember() {
        this.doneHouseholdMember = false;
        this.editingHouseholdMember = true;
    }
    SaveHouseholdMember() {
        // todo: implement a loop on the HTML for each family member
        // and take some sort of ID here
        this.editingHouseholdMember = false;
        this.doneHouseholdMember = false;
    }
    ContinueToAccountRep() {
        this.HideAll();
        this.showAccountRep = true;
        this.scrollToService.scrollTo("accountRep");
    }


    EditAccountRep() {
        this.doneAccountRep = false;
        this.editingAccountRep = false;
    }
    SaveAccountRep() {
        this.doneAccountRep = true;
        this.editingAccountRep = false;
    }
    ShowAvailableFunds() {
        this.showAccountRep = false;
        this.finishedHousehold.emit();
        this.showAvailableFunds.emit();
    }
}