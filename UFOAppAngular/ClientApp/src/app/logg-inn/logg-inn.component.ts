import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-logg-inn',
  templateUrl: './logg-inn.component.html',
  styleUrls: ['./logg-inn.component.css']
})
export class LoggInnComponent implements OnInit {
    skjema: FormGroup;

    //validering
    validering = {
        brukernavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        passord: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
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

        this.http.post("api/UFO/loggInn", bruker)
            .subscribe(retur => {
                this.router.navigate(['/observator-oversikt']);
            },
                error => console.log(error)
                //SLETT: her må det være litt mer tilbakemelding til brukeren om feil brukernavn/passord
            );
    }

}
