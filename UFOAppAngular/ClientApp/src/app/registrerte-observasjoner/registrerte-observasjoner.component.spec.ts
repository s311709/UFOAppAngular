import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrerteObservasjonerComponent } from './registrerte-observasjoner.component';

describe('IndexComponent', () => {
  let component: RegistrerteObservasjonerComponent;
  let fixture: ComponentFixture<RegistrerteObservasjonerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrerteObservasjonerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrerteObservasjonerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
