import { Component, ElementRef, ViewChild, Inject } from "@angular/core";
import { Http } from '@angular/http';

@Component({
    template: `
        <h2> CSV Uploader </h2>
        <p>it works </p>
        {{pageTitle}}
        <input type="file" #csvFileInput/>
        <input type="button" value="submit csv file" (click)="submitFile()" />
        <div>
            <input type="button" value="Test Connection" (click)="testConnection()" />
        </div>
        test Connection value: {{testValue}}
        <h4>Download csv</h4>
        <a (click)="downloadCsv()" >Download CSV</a>
    `
})
export class CSVUploaderComponent {

    pageTitle: string = 'csv stuff'
    @ViewChild("csvFileInput") csvFileInput: ElementRef;
    testValue: string = '';

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) { }

    submitFile() {
        const file = this.getFileAttachment()
        console.log(file.name);
        this.http.post(this.baseUrl + `api/csvUploader/uploadCsv`, this.buildFormData()).subscribe(res => {
            console.log(JSON.stringify(res));
            this.testValue = res.text();
        });
    }

    buildFormData(): FormData {
        let formData = new FormData();
        formData.append("CsvFile", this.getFileAttachment());
        return formData
    }

    testConnection() {
        this.http.get(this.baseUrl + `api/csvUploader`).subscribe(res => {
            console.log(JSON.stringify(res));
            this.testValue = res.text();
        });
    }

    fileChangeEvent(fileInput: any) {
        if (fileInput.target.files && fileInput.target.files[0]) {
            const file: File = fileInput.target.files[0];
        }
    }

    downloadCsv() {
        window.open('http://localhost:63047/api/csvUploader/account-data.csv', '_blank', '');
    }

    // To do:
    private getFileAttachment(): File {
        const file = this.csvFileInput.nativeElement;
        return file.files[0];
    }
}
