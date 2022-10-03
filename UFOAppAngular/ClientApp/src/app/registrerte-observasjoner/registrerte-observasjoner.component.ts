import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observasjon } from "../Observasjon";


@Component({
  selector: 'app-registrerte-observasjoner',
  templateUrl: './registrerte-observasjoner.component.html',
  styleUrls: ['./registrerte-observasjoner.component.css']
})
export class RegistrerteObservasjonerComponent {

    public alleObservasjoner: Array<Observasjon>;
    public laster: boolean;

    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleObservasjoner();
    }

    hentAlleObservasjoner() {
        //arrayet skal returnere et observasjonsarray
        this.http.get<Observasjon[]>("api/UFO/")
            .subscribe(observasjonene => {
                this.alleObservasjoner = observasjonene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };

}
