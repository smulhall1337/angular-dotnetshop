import { Component } from "@angular/core";
import { Output, EventEmitter } from '@angular/core';
import { ScrollToService } from "../services/scroll-to.service";
import { ApplicationComponent } from "./application.component";


@Component({
  selector: 'app-application-preamble',
  templateUrl: './application-preamble.component.html',
  styleUrls: ['./application.component.scss']
})
export class ApplicationPreambleComponent{
  acceptedPreamble: boolean = false;
  acceptedNeededInformation: boolean = false;
  acceptedImportantInformation: boolean = false;
  @Output() showHousehold: EventEmitter<any> = new EventEmitter();
  @Output() finishPreamble: EventEmitter<any> = new EventEmitter();

  constructor(private scrollToService: ScrollToService){}

  showNeededInformation: boolean = false;
  showImportantInformation: boolean = false;
  showPreamble: boolean = true;

  acceptPreamble() {
    this.acceptedPreamble = true;
    this.showNeededInformation = true;
    this.showPreamble = false;
    this.scrollToService.scrollTo("neededInformation")
  }

  BackToPreamble(){
    this.showNeededInformation = false;
    this.showPreamble = true;
    this.scrollToService.scrollTo("preamble")
  }

  AcceptNeededInformation() {
    this.acceptedNeededInformation = true;
    this.showNeededInformation = false;
    this.showImportantInformation = true;
    this.scrollToService.scrollTo("importantInformation")
  }

  BackToNeededInformation() {
    this.showImportantInformation = false;
    this.showNeededInformation = true;
    this.scrollToService.scrollTo("neededInformation")
  }

  ContinueToHousehold() {
    this.finishPreamble.emit('true');
    this.showHousehold.emit('true');
  }

  acceptImportantInformation() {
    this.acceptedImportantInformation = true;
    //this.showHousehold = true;
    // this.editingHousehold = true;
  }
}
