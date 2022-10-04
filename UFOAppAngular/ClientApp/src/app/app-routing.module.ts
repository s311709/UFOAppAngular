import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { RegistrerteObservasjonerComponent } from './registrerte-observasjoner/registrerte-observasjoner.component';
import { NavMenyComponent } from './nav-meny/nav-meny.component';
import { LoggInnComponent } from './logg-inn/logg-inn.component';
import { LagreObservasjonComponent } from './lagre-observasjon/lagre-observasjon.component';


//Deklarer appRoot med paths
const appRoots: Routes = [
    { path: 'registrerte-observasjoner', component: RegistrerteObservasjonerComponent },
    { path: 'lagre-observasjon', component: LagreObservasjonComponent },
    { path: 'nav-meny', component: NavMenyComponent },
    { path: 'logg-inn', component: LoggInnComponent },
    //Default eller localhost
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
