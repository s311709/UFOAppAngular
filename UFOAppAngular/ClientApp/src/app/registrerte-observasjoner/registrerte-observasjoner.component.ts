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
    observasjonTilSletting: string;
    slettingOK: boolean;

    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleObservasjoner();
    }

    hentAlleObservasjoner() {
        this.http.get<Observasjon[]>("api/UFO/HentAlleObservasjoner")
            .subscribe(observasjonene => {
                this.alleObservasjoner = observasjonene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };

    //Denne henter kallenavn og tidspunkt på observasjon som skal slettes
    slettObservasjon (id: number) {

        // henter kallenavn og dato på UFO observert
        this.http.get<Observasjon>("api/UFO/HentEnObservasjon/" + id)
            .subscribe(observasjon => {
                this.observasjonTilSletting = observasjon.kallenavnUFO + " " + observasjon.tidspunktObservert;

                // viser modalen og kaller til slett
                this.visModalOgSlett(id);
            },
                error => console.log(error)
            );
    }

    visModalOgSlett(id: number) {

        
    }

}
