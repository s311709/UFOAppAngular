import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Observasjon, Observator, UFO } from "../Observasjon";

@Component({
    selector: 'app-lagre-observasjon',
    templateUrl: './lagre-observasjon.component.html',
    styleUrls: ['./lagre-observasjon.component.css']
})
export class LagreObservasjonComponent {

    public alleUFOer: Array<UFO>;
    public modell: String;

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

    ngOnInit() {
      this.hentAlleUFOer();
    }

    hentAlleUFOer() {
        this.http.get<UFO[]>("api/UFO/HentAlleUFOer")
            .subscribe(ufoene => {
                this.alleUFOer = ufoene;
            },
                error => {
                    console.log(error);
                    let feil = document.getElementById("feil") as HTMLDivElement;
                    feil.innerHTML = "Det har oppstått en feil på server.";
                }
            );
    };

    /* følgende kode er for å kunne velge en allerede observert UFO
     * og deretter autofylle modell fra databasen
     */

    velgSettUFO(event) {
        /*cat holder verdien av det valgte elementet i dropdown 
         *kilde: 
         */
        let cat = event.target.options[event.target.options.selectedIndex].text;
        if (cat != "Ny UFO") {
            this.skjema.patchValue({ kallenavnUFO: event.target.options[event.target.options.selectedIndex].text });
            this.skjema.controls['kallenavnUFO'].disable();
            this.skjema.controls['modell'].disable();
            this.finnModellnavn(cat);
        }
        else {
            this.skjema.controls['kallenavnUFO'].enable();
            this.skjema.controls['modell'].enable();
            this.skjema.patchValue({ kallenavnUFO: "" });
            this.skjema.patchValue({ modell : "" });
        }
    }

   finnModellnavn(kallenavn: string) {
        this.http.get<UFO>("api/UFO/HentEnUfo/" + kallenavn)
            .subscribe(ufo => {
                this.skjema.patchValue({ modell: ufo.modell });
            },
                error => {
                    console.log(error);
                    let feil = document.getElementById("feil") as HTMLDivElement;
                    feil.innerHTML = "Det har oppstått en feil i henting av modell.";
                }
       );
    };

    //når etternavn endres, prøv å finn bruker i DB ved å sende inn fornavn og etternavn
    onChangeEtternavn() {
        /*disse fire linjene er for å låse opp/tømme autofill-feltene
         hvis man endrer etternavn igjen
         */
        this.skjema.controls['epostObservator'].enable();
        this.skjema.controls['telefonObservator'].enable();
        this.skjema.patchValue({ epostObservator: "" });
        this.skjema.patchValue({ telefonObservator: "" });
        let innFornavn = (<HTMLInputElement>document.getElementById("fornavn")).value;
        let innEtternavn = (<HTMLInputElement>document.getElementById("etternavn")).value;
        this.hentObservatorData(innFornavn, innEtternavn);
    }

    /* følgende kode er for å autofylle observatørdata
        basert på fornavn og etternavn fra registrerte observatører
     */
    hentObservatorData(fornavn, etternavn) {
        //hent ut data fra fornavn og etternavn og send til db
        this.http.get<Observator>("api/UFO/HentEnObservator/" + fornavn + "/" + etternavn)
            .subscribe(
                observator => {
                    this.skjema.patchValue({ fornavnObservator: observator.fornavn });
                    this.skjema.patchValue({ etternavnObservator: observator.etternavn });
                    this.skjema.patchValue({ telefonObservator: observator.telefon});
                    this.skjema.patchValue({ epostObservator: observator.epost });
                    this.skjema.controls['epostObservator'].disable();
                    this.skjema.controls['telefonObservator'].disable();
                },
                error => console.log(error)
            );
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
                    error => {
                        console.log(error);
                        let feil = document.getElementById("feil") as HTMLDivElement;
                        feil.innerHTML = "Det har oppstått en feil på server.";
                    }
                );
        }
    };

}
