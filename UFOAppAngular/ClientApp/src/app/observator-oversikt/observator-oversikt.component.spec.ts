import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservatorOversiktComponent } from './observator-oversikt.component';

describe('ObservatorOversiktComponent', () => {
  let component: ObservatorOversiktComponent;
  let fixture: ComponentFixture<ObservatorOversiktComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservatorOversiktComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservatorOversiktComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
