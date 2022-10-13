export class Observasjon {
    id: number;
    kallenavnUFO: string;
    tidspunktObservert: Date;
    kommuneObservert: string;
    beskrivelseAvObservasjon: string;
    modell: string;
    fornavnObservator: string;
    etternavnObservator: string;
    telefonObservator: string;
    epostObservator: string;
}

export class UFO {
    id: number;
    kallenavn: string;
    modell: string;
    gangerObservert: string;
    sistObservert: Date;
}

export class Observator {
    id: number;
    fornavn: string;
    etternavn: string;
    telefon: string;
    epost: string;
    antallRegistrerteObservasjoner: number;
    sisteObservasjon: Date;
}

export class Bruker {
    brukernavn: string;
    passord: string;
}

