﻿import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Observasjon } from "../Observasjon";

@Component({
    selector: 'app-lagre-observasjon',
    templateUrl: './lagre-observasjon.component.html',
    styleUrls: ['./lagre-observasjon.component.css']
})
export class LagreObservasjonComponent {

    skjema: FormGroup;
    dato = new Date(); //dato = idag
    UTCdato = new Date(this.dato.setHours(this.dato.getHours() + 2)); //for å få riktig tid i forhold til UTC+2
    datoJSON = this.UTCdato.toJSON(); //konverterer til JSON-objekt for å sammenlikne med kalender-input senere

    visDatoFeil: boolean = false;

    validering = {
        id: [""],
        kallenavnUFO: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        tidspunktObservert: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ:\\-. ]{2,30}")])
        ],
        kommuneObservert: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        beskrivelseAvObservasjon: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,250}")])
        ],
        modell: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        fornavnObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        etternavnObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        telefonObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9]{8}")])
        ],
        epostObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-.@ ]{2,30}")])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        this.skjema = fb.group(this.validering);
    }

    vedSubmit() {
        this.lagreObservasjon();
    }


    sjekkDato(): boolean {
        if (this.datoJSON < this.skjema.value.tidspunktObservert) {
            this.visDatoFeil = true;
            return false;
        }
        if (this.datoJSON > this.skjema.value.tidspunktObservert) {
            this.visDatoFeil = false;
            return true;
        }
        return false;
    }

    lagreObservasjon() {
        if (this.sjekkDato() == true) { //sender bare skjema hvis dato også stemmer 
            const lagretObservasjon = new Observasjon();

            lagretObservasjon.kallenavnUFO = this.skjema.value.kallenavnUFO;
            lagretObservasjon.tidspunktObservert = this.skjema.value.tidspunktObservert;
            lagretObservasjon.kommuneObservert = this.skjema.value.kommuneObservert;
            lagretObservasjon.beskrivelseAvObservasjon = this.skjema.value.beskrivelseAvObservasjon;
            lagretObservasjon.modell = this.skjema.value.modell;
            lagretObservasjon.fornavnObservator = this.skjema.value.fornavnObservator;
            lagretObservasjon.etternavnObservator = this.skjema.value.etternavnObservator;
            lagretObservasjon.telefonObservator = this.skjema.value.telefonObservator;
            lagretObservasjon.epostObservator = this.skjema.value.epostObservator;

            this.http.post("api/UFO/LagreObservasjon", lagretObservasjon)
                .subscribe(retur => {
                    this.router.navigate(['/registrerte-observasjoner']);
                },
                    error => console.log(error)
                );
        }
    };

}
