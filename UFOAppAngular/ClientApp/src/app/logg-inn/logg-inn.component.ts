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
        ],
        passord: [

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
                if (retur == true) {
                    this.router.navigate(['/registrerte-observasjoner']);
                } else if (retur == false) {
                    console.log("Feil brukernavn eller passord");
                    let feil = document.getElementById("feil") as HTMLDivElement;
                    feil.innerHTML = "Feil brukernavn eller passord";
                }
            },
                error => console.log(error)
            );
    }

}
