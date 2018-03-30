import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { CSVUploaderComponent } from './components/csv-uploader/csv-uploader.component';
import { ToastComponent } from './components/toast/toast.component';
import { AlertService } from './components/alert/alert.service';
import { AlertComponent } from './components/alert/alert.component';
import { ClipboardComponent } from './components/clipboard/clipboard.component';
import { ClipboardModule } from 'ngx-clipboard';
import { FilterTestComponent } from './components/filter/filter-test.component';
import { OrderModule } from 'ngx-order-pipe';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        CSVUploaderComponent,
        ToastComponent,
        AlertComponent,
        ClipboardComponent,
        FilterTestComponent,
        HomeComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        ClipboardModule,
        OrderModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: 'csv-uploader', component: CSVUploaderComponent },
            { path: 'toast', component: ToastComponent },
            { path: 'clipboard', component: ClipboardComponent },
            { path: 'filter-test', component: FilterTestComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [AlertService]
})
export class AppModuleShared {
}
