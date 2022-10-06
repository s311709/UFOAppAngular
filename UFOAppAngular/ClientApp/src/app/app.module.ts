import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


import { AppComponent } from './app.component';
import { RegistrerteObservasjonerComponent } from './registrerte-observasjoner/registrerte-observasjoner.component';
import { AppRoutingModule } from './app-routing.module';
import { NavMenyComponent } from './nav-meny/nav-meny.component';
import { LoggInnComponent } from './logg-inn/logg-inn.component';
import { LagreObservasjonComponent } from './lagre-observasjon/lagre-observasjon.component';
import { EndreObservasjonComponent } from './endre-observasjon/endre-observasjon.component';
import { Modal } from './registrerte-observasjoner/sletteModal';


@NgModule({
    declarations: [
        AppComponent,
        RegistrerteObservasjonerComponent,
        NavMenyComponent,
        LoggInnComponent,
        LagreObservasjonComponent,
        EndreObservasjonComponent,
        Modal
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        AppRoutingModule,
        ReactiveFormsModule,
        NgbModule
    ],
    providers: [],
    bootstrap: [AppComponent],
    entryComponents: [Modal]
})
export class AppModule { }
