import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UfoOversiktComponent } from './ufo-oversikt.component';

describe('UfoOversiktComponent', () => {
  let component: UfoOversiktComponent;
  let fixture: ComponentFixture<UfoOversiktComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UfoOversiktComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UfoOversiktComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
