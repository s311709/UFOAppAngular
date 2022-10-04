import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LagreObservasjonComponent } from './lagre-observasjon.component';

describe('LagreObservasjonComponent', () => {
  let component: LagreObservasjonComponent;
  let fixture: ComponentFixture<LagreObservasjonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LagreObservasjonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LagreObservasjonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
