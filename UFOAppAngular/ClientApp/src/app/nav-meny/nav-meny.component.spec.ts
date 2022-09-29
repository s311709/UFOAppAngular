import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NavMenyComponent } from './nav-meny.component';

describe('NavMenyComponent', () => {
  let component: NavMenyComponent;
  let fixture: ComponentFixture<NavMenyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NavMenyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavMenyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
