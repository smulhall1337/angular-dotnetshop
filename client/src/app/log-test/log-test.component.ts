import { Component } from "@angular/core";
import { LoggingService } from '../shared/logging/logging.service';

@Component({
    selector: "log-test",
    templateUrl: "./log-test.component.html"
})
export class LogTestComponent {
    constructor(private logger: LoggingService) {
    }

    testLog(): void {
        this.logger.log("Test the `log()` Method");
    }
}