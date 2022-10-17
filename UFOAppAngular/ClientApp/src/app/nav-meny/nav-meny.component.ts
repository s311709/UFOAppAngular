import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-meny',
  templateUrl: './nav-meny.component.html',
  styleUrls: ['./nav-meny.component.css']
})
export class NavMenyComponent implements OnInit {

  loggetInn: boolean;

    ngOnInit(){
    }

    constructor(private http: HttpClient, private router: Router) { }

    loggUt() {
        this.http.get<any>("api/UFO/LoggUt")
            .subscribe(retur => {
                if (retur == true) {
                    console.log("Du har blitt logget ut.")
                    this.router.navigate(['/logg-inn']);
                }
                else {
                    alert("Du er allerede logget ut.")
                }
            },
                error => console.log(error)
            );
    };

}
