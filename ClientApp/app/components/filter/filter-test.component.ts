import { Component } from "@angular/core";

@Component({
    template: `
        <ul>
            <li *ngFor="let product of thousandProducts | orderBy: order : toggleReverse ">
                {{ product.name }} &nbsp;
                {{ product.quantity }} &nbsp;
                {{ product.dateCreated | date:"MM-dd-yyyy"}} &nbsp;
            </li>
        </ul>
        <a class="btn btn-primary" (click)="sort('name')"> Sort by Name </a>
        <a class="btn btn-secondary" (click)="sort('quantity')"> Sort by Quantity</a>
        <a class="btn btn-secondary" (click)="sort('id')"> Sort by Id</a>
        <a class="btn btn-secondary" (click)="sort('dateCreated')"> Sort by Date</a>
    `
})
export class FilterTestComponent {

    thousandProducts: any[] = [];

    constructor() {
        let index = 1;
        while (this.thousandProducts.length != 1000) {
            this.thousandProducts.push({
                id: index,
                name: 'Super Soap ' + index,
                quantity: 25 * index,
                dateCreated: new Date().setFullYear(2000 - index, 6, 23)
            });
            index++;
        }
    }

    toggleReverse: boolean = false;

    sort(key: string = "name") {
        this.toggleReverse = !this.toggleReverse;
        this.order = key;
    }

    products: any[] = [{
        id: 1,
        name: 'Super Soap',
        quantity: 5000,
        dateCreated: new Date().setFullYear(2018, 6, 23)
    }, {
        id: 2,
        name: 'Banana Launcher',
        quantity: 30,
        dateCreated: new Date().setFullYear(2012, 6, 23)
    },
    {
        id: 3,
        name: 'Great Pike',
        quantity: 3000,
        dateCreated: new Date().setFullYear(2015, 6, 23)
    },
    {
        id: 4,
        name: 'Shield of Nostradame',
        quantity: 250,
        dateCreated: new Date().setFullYear(2017, 6, 23)
    },
    {
        id: 5,
        name: 'Boots of Openhagen',
        quantity: 600,
        dateCreated: new Date().setFullYear(2008, 6, 23)
    }
    ];
    order: string = 'name'
}