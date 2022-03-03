/**
 * TODO:
 *  1. implement cookie logic. See 'cookie' region
 */
import { ViewportScroller } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.scss'],
})
export class ApplicationComponent implements OnInit {





  //#region section flags 

  editingAvailableFunds: boolean = false;
  finishedAvailableFunds: boolean = false;





  // #region preamble
  showPreamble: boolean = true;
  finishedPreamble: boolean = false;
  @Input() showHousehold: boolean = false;
  @Input() acceptedPreamble: boolean = false;
  // #endregion

  // #region household info
  doneHouseholdInfo: boolean = false;
  finishedHousehold: boolean = false;
  editingHousehold: boolean = false;


  showAvailableFunds: boolean = false;
  showIncome: boolean = false;
  editingIncome: boolean = false;
  finishedIncome: boolean = false;

  showExpenses: boolean = false;
  editingExpenses: boolean = false;
  finishedExpenses: boolean = false;


  showSummary: boolean = false;






  // #endregion section flags

  currentSection = 'section1';
  btn = document.querySelector("button.mobile-menu-button");
  menu = document.querySelector(".mobile-menu");

  constructor(
    private viewportScroller: ViewportScroller
  ) {

  }

  ngOnInit(): void {

  }

  change(value: number): void {
    console.log(value);
  }

  HideAll() {
    this.showHousehold = false;
    this.showIncome = false;
    this.showPreamble = false;
    this.showSummary = false;
    this.showAvailableFunds = false;
    this.showExpenses = false;
  }

  // #region Show functions
  ShowPreamble(event: any) {
    this.HideAll();
    this.showPreamble = true;
  }
  ShowHousehold(event: any) {
    this.HideAll();
    this.showHousehold = true;
  }
  ShowAvailableFunds(event: any) {
    this.HideAll();
    this.showAvailableFunds = true;
  }
  ShowIncome(event: any) {
    this.HideAll();
    this.showIncome = true;
  }
  ShowExpenses(event: any) {
    this.HideAll();
    this.showExpenses = true;
  }
  ShowSummary(event: any) {
    this.HideAll();
    this.showSummary = true;
  }
  // #endregion

  // #region Finish functions
  FinishPreamble(event: any) {
    this.finishedPreamble = true;
  }
  FinishHousehold(event: any) {
    this.finishedHousehold = true;
  }
  FinishAvailableFunds(event: any) {
    this.finishedAvailableFunds = true;
  }
  FinishIncome(event: any) {
    this.finishedIncome = true;
  }
  FinishExpenses(event: any) {
    this.finishedExpenses = true;
  }
  // #endregion

  // #region LexisNexis modal
  showLexisNexisModal: boolean = false;
  display = 'none';
  ShowLexisNexisModal() {
    this.display = 'block';
    // I like overlyspecific function names. sue me.
  }
  CloseLexisNexisModal() {
    this.display = 'none';
  }

  /*
  HandleIdentityAuth() {
    // WARNING: JAVASCRIPT AHEAD!!!1 
    // all this stuff was ripped straight out of the previous app
    let _uri = "https://localhost:44368/"; // For testing LexisNexis locally

    let userId = ""// document.getElementById("fa-userid-el").innerHTML;
    let myAlUserId = "" //document.getElementById("myalabama-id-el").innerHTML;
    if (myAlUserId) {
      userId = myAlUserId;
    }
    let uri = _uri + "user/" + userId;

    if (window) {
      if (navigator.userAgent.toLowerCase().indexOf("chrome") > -1) {
        var popup = this.popupWindow(uri, "MyDHR Identity Authentication", window, 950, 950);

        setTimeout(function () {
          popup.focus();
          this.handlePopupUnload(popup);
        }, 250);

        popup.onfocus = function () {
          setTimeout(function () {
            popup.focus();
            this.handlePopupUnload(popup);
          }, 200);
        };
      } else {
        var popup = this.popupWindow(uri, "MyDHR Identity Authentication", window, 1200, 1200);

        popup.focus();

        var timer = setInterval(function () {
          if (popup.closed) {

            $("#popup-active").hide();
            $("#ln-section-content-complete").show();
            $(".ln-section-content").hide();

            setTimeout(function () {
              $("#ln-overlay").hide();

              setTimeout(function () {
                clearInterval(timer);
                window.location.reload();
              }, 250);
            }, 4500);
          }
        }, 1000);
      }
    }
  }
  popupWindow(url: any, windowName: any, win: any, w: any, h: any) {
    var y = win.top.outerHeight / 2 + win.top.screenY - (h / 2);
    var x = win.top.outerWidth / 2 + win.top.screenX - (w / 2);
    return win.open(url, windowName, "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=" + w + ", height=" + h + ", top=" + y + ", left=" + x + "");
  }
  handlePopupUnload(win: any) {
    var timer = setInterval(function () {
      if (win.closed) {
        clearInterval(timer);
        $("#popup-active").hide();
        $("#ln-section-content-complete").show();
        $(".ln-section-content").hide();

        setTimeout(function () {
          $("#ln-overlay").hide();

          setTimeout(function () {
            window.location.reload();
          }, 250);
        }, 4500);
      }
    }, 1000);
  }
  */

  // #endregion

  // #region Cookies
  SetOptOutCookie() {
    // $("#accountOptOut").val(true);
    this.SetItem(document.getElementById("fa-userid-el")!.innerHTML + "_OptOut", true);
    this.SetItem(document.getElementById("fa-userid-el")!.innerHTML + "_OptIn", false);
  }

  SetOptInCookie() {
    //$("#accountOptOut").val(false);
    this.SetItem(document.getElementById("fa-userid-el")!.innerHTML + "_OptOut", false);
    this.SetItem(document.getElementById("fa-userid-el")!.innerHTML + "_OptIn", true);
  }

  SetCompletedAppCookie() {
    this.SetItem(document.getElementById("fa-userid-el")!.innerHTML + "_CompletedAuth", "completedIdentityAuth");
  }

  SetFullyCompletedCookie() {
    this.SetItem(document.getElementById("fa-userid-el")!.innerHTML + "_FullyCompleted", "fullyCompletedApplication");
  }

  SetItem(name: any, value?: any, days?: any) {
    localStorage.setItem(name, value);
  }
  // #endregion


}
