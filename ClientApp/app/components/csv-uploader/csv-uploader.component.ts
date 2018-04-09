import { Component, ElementRef, ViewChild, Inject } from "@angular/core";
import { Http } from '@angular/http';

@Component({
    template: `
        <h2> CSV Uploader </h2>
        <p>it works </p>
        {{pageTitle}}
        <input type="file" #csvFileInput (change)="fileChangeEvent($event)"/>
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
    testValue: string = '';

    @ViewChild("csvFileInput") csvFileInput: ElementRef;

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
        let fileReader = new FileReader();

        fileReader.onload = () => {
            const text = fileReader.result;
            this.extractText(text);
            console.log(text);
        }

        if (fileInput.target.files && fileInput.target.files[0]) {
            const file: File = fileInput.target.files[0];
            fileReader.readAsText(file);
        }
    }

    extractText(text: string) {
        const rows = text.split('\n');
        if (rows && rows.length > 0) {
            // Check Header if applicable
            // E.g: FirstName,LastName,Email
            const header = rows[0];
            const headerProps = header.split(',');
            if (headerProps.length != 3) {
                console.error(`invalid csv format: make sure the first line of the csv is in this format: 'FirstName,LastName,Email'`);
                return;
            }

            // Check if all lines/entry has firstName, last name and email
            rows.forEach(row => {
                if (row) {
                    const rowProps = row.split(',');
                    const isCorrectFormat = rowProps.length == 3;
                    if (!isCorrectFormat) {
                        console.error(`This '${row}' is invalid.. Please follow this format: 'FirstName,LastName,Email' e.g.: test,testLast,testEmail@someemail.com`);
                    }
                }
            });

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
