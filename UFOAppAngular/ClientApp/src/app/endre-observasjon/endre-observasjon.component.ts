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

    validering = {
        id: [""],
        kallenavnUFO: [
            null
        ],
        tidspunktObservert: [
            null
        ],
        kommuneObservert: [
            null
        ],
        beskrivelseAvObservasjon: [
            null
        ],
        modell: [
            null
        ],
        fornavnObservator: [
            null
        ],
        etternavnObservator: [
            null
        ],
        telefonObservator: [
            null
        ],
        epostObservator: [
            null
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
                error => console.log(error)
            );
    }
}
