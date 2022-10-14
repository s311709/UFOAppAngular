import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-nav-meny',
  templateUrl: './nav-meny.component.html',
  styleUrls: ['./nav-meny.component.css']
})
export class NavMenyComponent implements OnInit {

  loggetInn: boolean;

    ngOnInit(){
        this.sjekkLoggetInn();
    }

    constructor(private http: HttpClient) { }

    sjekkLoggetInn() {
        
        this.http.get<any>("api/UFO/SjekkLoggetInn")
            .subscribe(retur => {
                if (retur == true) {
                    document.getElementById("skjultNav").hidden = true;
                } else if (retur == false) {
                    document.getElementById("skjultNav").hidden = false;
                }
            },
                error => console.log(error)
            );
    }

}
