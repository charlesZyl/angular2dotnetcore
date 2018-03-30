import { Component } from "@angular/core";
import { AlertService } from "../alert/alert.service";


@Component({
    templateUrl: './toast.component.html'
})
export class ToastComponent{

    pageTitle: string = 'Toast Component Title';

    constructor(private alertService: AlertService) { }

    success(message: string) {
        this.alertService.success(message);
    }

    error(message: string) {
        this.alertService.error(message);
    }

    info(message: string) {
        this.alertService.info(message);
    }

    warn(message: string) {
        this.alertService.warn(message);
    }

    clear() {
        this.alertService.clear();
    }

}