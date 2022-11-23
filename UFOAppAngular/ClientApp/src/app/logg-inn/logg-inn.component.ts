import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { Bruker } from "../Observasjon";

//Denne brukes for å lese http-respons


@Component({
  selector: 'app-logg-inn',
  templateUrl: './logg-inn.component.html',
  styleUrls: ['./logg-inn.component.css']
})
export class LoggInnComponent {
    skjema: FormGroup;

    //validering
    validering = {
        brukernavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        passord: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{4,30}$")])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        this.skjema = fb.group(this.validering);
    }

    vedSubmit() {
        this.loggInn();
    }

    loggInn() {
        const bruker = new Bruker();

        bruker.brukernavn = this.skjema.value.brukernavn;
        bruker.passord = this.skjema.value.passord;

        this.http.post<any>("api/UFO/LoggInn", bruker)
            .subscribe(retur => {
              //  this.router.navigate(['/registrerte-observasjoner']);
                let feil = document.getElementById("feil") as HTMLDivElement;
                feil.innerHTML = "";
            },
                error => {
                    console.log(error);
                    let feil = document.getElementById("feil") as HTMLDivElement;
                    feil.innerHTML = "Feil brukernavn eller passord.";
                }
            );
    }

}
