import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { RegistrerteObservasjonerComponent } from './registrerte-observasjoner/registrerte-observasjoner.component';
import { AppRoutingModule } from './app-routing.module';
import { NavMenyComponent } from './nav-meny/nav-meny.component';
import { LoggInnComponent } from './logg-inn/logg-inn.component';
import { LagreObservasjonComponent } from './lagre-observasjon/lagre-observasjon.component';
import { EndreObservasjonComponent } from './endre-observasjon/endre-observasjon.component';

@NgModule({
    declarations: [
        AppComponent,
        RegistrerteObservasjonerComponent,
        NavMenyComponent,
        LoggInnComponent,
        LagreObservasjonComponent,
        EndreObservasjonComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        AppRoutingModule,
        ReactiveFormsModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
