import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { Bruker } from "../Observasjon";

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
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        passord: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
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

        this.http.post("api/UFO/LoggInn", bruker)
            .subscribe(retur => {
                this.router.navigate(['/observator-oversikt']);
            },
                error => console.log(error)
                //SLETT: her m� det v�re litt mer tilbakemelding til brukeren om feil brukernavn/passord
            );
    }

}
