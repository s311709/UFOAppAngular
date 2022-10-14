import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { UFO } from "../Observasjon";


@Component({
  selector: 'app-ufo-oversikt',
  templateUrl: './ufo-oversikt.component.html',
  styleUrls: ['./ufo-oversikt.component.css']
})
export class UfoOversiktComponent {

    public alleUFOer: Array<UFO>;
    public laster: boolean;

    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleUFOer();
  }
    hentAlleUFOer() {
        this.http.get<UFO[]>("api/UFO/HentAlleUFOer")
            .subscribe(ufoene => {
                this.alleUFOer = ufoene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };
}
