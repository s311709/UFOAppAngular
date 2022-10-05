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
        fornavnObservator: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
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

        lagretObservasjon.fornavnObservator = this.skjema.value.fornavnObservator;
        
        this.http.post("api/UFO/", lagretObservasjon)
            .subscribe(retur => {
                this.router.navigate(['/registrerte-observasjoner']);
            },
                error => console.log(error)
            );
    };

}
