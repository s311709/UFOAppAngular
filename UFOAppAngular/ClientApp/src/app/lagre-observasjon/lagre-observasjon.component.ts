import { Component } from '@angular/core';
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

    validering = {
        id: [""],
        kallenavnUFO: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        tidspunktObservert: [
            null
        ],
        kommuneObservert: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        beskrivelseAvObservasjon: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        modell: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        fornavnObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        etternavnObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        telefonObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ],
        epostObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z������\\-. ]{2,30}")])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        this.skjema = fb.group(this.validering);
    }

    vedSubmit() {
        this.lagreObservasjon();
    }

    lagreObservasjon() {
        const lagretObservasjon = new Observasjon();

        lagretObservasjon.kallenavnUFO = this.skjema.value.kallenavnUFO;
        lagretObservasjon.tidspunktObservert = this.skjema.value.tidspunktObservert;
        lagretObservasjon.kommuneObservert = this.skjema.value.kommuneObservert;
        lagretObservasjon.beskrivelseAvObservasjon = this.skjema.value.fornavnObservator;
        lagretObservasjon.modell = this.skjema.value.fornavnObservator;
        lagretObservasjon.fornavnObservator = this.skjema.value.fornavnObservator;
        lagretObservasjon.etternavnObservator = this.skjema.value.etternavnObservator;
        lagretObservasjon.telefonObservator = this.skjema.value.fornavnObservator;
        lagretObservasjon.epostObservator = this.skjema.value.fornavnObservator;

        this.http.post("api/UFO/LagreObservasjon", lagretObservasjon)
            .subscribe(retur => {
                this.router.navigate(['/registrerte-observasjoner']);
            },
                error => console.log(error)
            );
    };

}
