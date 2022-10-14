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
    }

    constructor() { }

}
