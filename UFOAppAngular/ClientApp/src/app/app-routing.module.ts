import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'
import { IndexComponent } from './index/index.component';
import { NavMenyComponent } from './nav-meny/nav-meny.component';


//Deklarer appRoot med paths
const appRoots: Routes = [
    { path: 'index', component: IndexComponent },
    { path: 'nav-meny', component: NavMenyComponent },
    //Default eller localhost
    { path: '', redirectTo: 'home', pathMatch: 'full' }
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
