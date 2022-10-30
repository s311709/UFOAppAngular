import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { RegistrerteObservasjonerComponent } from './registrerte-observasjoner/registrerte-observasjoner.component';
import { NavMenyComponent } from './nav-meny/nav-meny.component';
import { LoggInnComponent } from './logg-inn/logg-inn.component';
import { LagreObservasjonComponent } from './lagre-observasjon/lagre-observasjon.component';
import { EndreObservasjonComponent } from './endre-observasjon/endre-observasjon.component';
import { UfoOversiktComponent } from './ufo-oversikt/ufo-oversikt.component';
import { ObservatorOversiktComponent } from './observator-oversikt/observator-oversikt.component';


//Deklarer appRoot med paths
const appRoots: Routes = [
    { path: 'registrerte-observasjoner', component: RegistrerteObservasjonerComponent },
    { path: 'lagre-observasjon', component: LagreObservasjonComponent },
    { path: 'nav-meny', component: NavMenyComponent },
    { path: 'logg-inn', component: LoggInnComponent },
    { path: 'endre-observasjon/:id', component: EndreObservasjonComponent },
    { path: 'ufo-oversikt', component: UfoOversiktComponent },
    { path: 'observator-oversikt', component: ObservatorOversiktComponent },
    //Logg-inn er default
    { path: '', redirectTo: 'logg-inn', pathMatch: 'full' }
    ]
//importerer og eksporterer RouterModule
@NgModule({
    declarations: [],
    imports: [
        RouterModule.forRoot(appRoots),
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }
