import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Observasjon } from "../Observasjon";
import * as moment from 'moment';

@Component({
  selector: 'app-endre-observasjon',
  templateUrl: './endre-observasjon.component.html',
  styleUrls: ['./endre-observasjon.component.css']
})
export class EndreObservasjonComponent implements OnInit {
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

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router, private route: ActivatedRoute) {
        this.skjema = fb.group(this.validering);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.endreObservasjon(params.id);
        })
    }
    vedSubmit() {
        this.endreEnObservasjon();
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

    endreObservasjon(id: number) {
        this.http.get<Observasjon>("api/UFO/HentEnObservasjon/" + id)
            .subscribe(
                observasjon => {
                    this.skjema.patchValue({ id: observasjon.id });
                    this.skjema.patchValue({ kallenavnUFO: observasjon.kallenavnUFO });
                    this.skjema.patchValue({ tidspunktObservert: observasjon.tidspunktObservert});
                    this.skjema.patchValue({ kommuneObservert: observasjon.kommuneObservert });
                    this.skjema.patchValue({ beskrivelseAvObservasjon: observasjon.beskrivelseAvObservasjon });
                    this.skjema.patchValue({ fornavnObservator: observasjon.fornavnObservator });
                    this.skjema.patchValue({ etternavnObservator: observasjon.etternavnObservator });
                    this.skjema.patchValue({ modell: observasjon.modell });
                    this.skjema.patchValue({ telefonObservator: observasjon.telefonObservator });
                    this.skjema.patchValue({ epostObservator: observasjon.epostObservator });
                },
                error => console.log(error)
            );
    }

    endreEnObservasjon() {
        if (this.sjekkDato() == true) { //sender bare skjema hvis dato også stemmer 

            const endretObservasjon = new Observasjon();
            endretObservasjon.id = this.skjema.value.id;
            endretObservasjon.kallenavnUFO = this.skjema.value.kallenavnUFO;
            endretObservasjon.tidspunktObservert = this.skjema.value.tidspunktObservert;
            endretObservasjon.kommuneObservert = this.skjema.value.kommuneObservert;
            endretObservasjon.beskrivelseAvObservasjon = this.skjema.value.beskrivelseAvObservasjon;
            endretObservasjon.fornavnObservator = this.skjema.value.fornavnObservator;
            endretObservasjon.etternavnObservator = this.skjema.value.etternavnObservator;
            endretObservasjon.modell = this.skjema.value.modell;
            endretObservasjon.telefonObservator = this.skjema.value.telefonObservator;
            endretObservasjon.epostObservator = this.skjema.value.epostObservator;

            this.http.put("api/UFO/EndreObservasjon", endretObservasjon)
                .subscribe(
                    retur => {
                        this.router.navigate(['/registrerte-observasjoner']);
                    },
                    error => {
                        console.log(error);
                        let feil = document.getElementById("feil") as HTMLDivElement;
                        feil.innerHTML = "Det har oppstått en feil på server.";
                    }
                );
        }
    }
}
