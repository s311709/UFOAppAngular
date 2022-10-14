import { HttpClient } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from './sletteModal';


import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observasjon } from "../Observasjon";


@Component({
  selector: 'app-registrerte-observasjoner',
  templateUrl: './registrerte-observasjoner.component.html',
  styleUrls: ['./registrerte-observasjoner.component.css']
})
export class RegistrerteObservasjonerComponent {

    public alleObservasjoner: Array<Observasjon>;
    public laster: boolean;
    UFOTilSletting: string;
    DatoTilSletting: Date;
    slettingOK: boolean;

    constructor(private http: HttpClient, private router: Router, private modalService: NgbModal) { }

    ngOnInit() {
        this.laster = true;
        this.hentAlleObservasjoner();
    }

    hentAlleObservasjoner() {
        this.http.get<Observasjon[]>("api/UFO/HentAlleObservasjoner")
            .subscribe(observasjonene => {
                this.alleObservasjoner = observasjonene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };

    //Denne henter kallenavn og tidspunkt på observasjon som skal slettes
    slettObservasjon (id: number) {

        // henter kallenavn og dato på UFO observert
        this.http.get<Observasjon>("api/UFO/HentEnObservasjon/" + id)
            .subscribe(observasjon => {
                this.UFOTilSletting = observasjon.kallenavnUFO;
                this.DatoTilSletting = observasjon.tidspunktObservert;

                // viser modalen og kaller til slett
                this.visModalOgSlett(id);
            },
                error => console.log(error)
            );
    }

    visModalOgSlett(id: number) {
        const modalRef = this.modalService.open(Modal);

        modalRef.componentInstance.kallenavnUFO = this.UFOTilSletting;
        modalRef.componentInstance.tidspunktObservert = this.DatoTilSletting;

        modalRef.result.then(retur => {
            console.log('Lukket med:' + retur);
            if (retur == "Slett") {

                // kall til server for sletting
                this.http.delete("api/UFO/SlettObservasjon/" + id)
                    .subscribe(retur => {
                        this.hentAlleObservasjoner();
                    },
                        error => console.log(error)
                    );
            }
            this.router.navigate(['/registrerte-observasjoner']);
        });
    }

}
