import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EndreObservasjonComponent } from './endre-observasjon.component';

describe('EndreObservasjonComponent', () => {
  let component: EndreObservasjonComponent;
  let fixture: ComponentFixture<EndreObservasjonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EndreObservasjonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EndreObservasjonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
