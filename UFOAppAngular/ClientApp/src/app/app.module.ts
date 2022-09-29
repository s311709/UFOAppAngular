import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { RegistrerteObservasjonerComponent } from './registrerte-observasjoner/registrerte-observasjoner.component';
import { AppRoutingModule } from './app-routing.module';
import { NavMenyComponent } from './nav-meny/nav-meny.component';
import { LoggInnComponent } from './logg-inn/logg-inn.component';

@NgModule({
  declarations: [
    AppComponent,
    RegistrerteObservasjonerComponent,
    NavMenyComponent,
    LoggInnComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
