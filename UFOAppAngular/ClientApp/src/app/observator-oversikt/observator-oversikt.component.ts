import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { Observator } from "../Observasjon";

@Component({
  selector: 'app-observator-oversikt',
  templateUrl: './observator-oversikt.component.html',
  styleUrls: ['./observator-oversikt.component.css']
})
export class ObservatorOversiktComponent implements OnInit {

    public alleObservatorer: Array<Observator>;
    public laster: boolean;

    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleObservatorer();
    }
    hentAlleObservatorer() {
        this.http.get<Observator[]>("api/UFO/HentAlleObservatorer")
            .subscribe(observatorene => {
                this.alleObservatorer = observatorene;
                this.laster = false;
            },
                error => {
                    console.log(error);
                    let feil = document.getElementById("feil") as HTMLDivElement;
                    feil.innerHTML = "Det har oppstått en feil på server.";
                }
            );
    };
}
