"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var registrerte_observasjoner_component_1 = require("./registrerte-observasjoner.component");
describe('IndexComponent', function () {
    var component;
    var fixture;
    beforeEach((0, testing_1.async)(function () {
        testing_1.TestBed.configureTestingModule({
            declarations: [registrerte_observasjoner_component_1.RegistrerteObservasjonerComponent]
        })
            .compileComponents();
    }));
    beforeEach(function () {
        fixture = testing_1.TestBed.createComponent(registrerte_observasjoner_component_1.RegistrerteObservasjonerComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=registrerte-observasjoner.component.spec.js.map