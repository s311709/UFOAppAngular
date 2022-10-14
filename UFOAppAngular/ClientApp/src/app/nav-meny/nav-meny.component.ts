import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-nav-meny',
    templateUrl: './nav-meny.component.html',
    styleUrls: ['./nav-meny.component.css']
})
export class NavMenyComponent implements OnInit {

    loggetInn: boolean;

    ngOnInit() {
        this.sjekkLoggetInn();
    }

    constructor(private http: HttpClient) { }

    sjekkLoggetInn() {

        this.http.get<any>("api/UFO/SjekkLoggetInn")
            .subscribe(retur => {
                if (retur == true) {
                    console.log("logget inn nå");
                    document.getElementById("#loggetInn").hidden = false;
                } else if (retur == false) {
                    console.log("ikke logget inn nå");
                    document.getElementById("#loggetInn").hidden = true;

                }
            },
                error => console.log(error)
            );
    }

}

/*
if (retur == true) {
    const navElement = document.querySelectorAll<HTMLElement>(".skjultNav")!;
    navElement.forEach(function (element) {
        (element as HTMLElement).hidden = true;
    })
} else if (retur == false) {
    const navElement = document.querySelectorAll<HTMLElement>(".skjultNav")!;
    navElement.forEach(function (element) {
        (element as HTMLElement).hidden = false;
    }) 
    */