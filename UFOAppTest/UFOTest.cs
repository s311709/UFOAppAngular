using System;
using Xunit;
using Moq; 
using UFOAppAngular.Controllers; // må legge til en prosjektreferanse i Project-> Add Reference -> Project
using UFOAppAngular.DAL;
using System.Threading.Tasks;
using System.Collections.Generic;
using UFOAppAngular.Models;

namespace UFOAppTest
{
    public class UFOControllerTest
    {
        [Fact]
        public async Task HentAlleObservasjonerLoggetInn()
        {
            var observasjon1 = new Observasjon
            {
                Id = 1,
                KallenavnUFO = "KornUFOen i Stavanger",
                TidspunktObservert = new DateTime(2022 - 01 - 01),
                KommuneObservert = "Stavanger",
                BeskrivelseAvObservasjon = "UFOen fløy i sirkler over byen.",
                FornavnObservator = "Heidi",
                EtternavnObservator = "Lyngås"
            };
            var observasjon2 = new Observasjon
            {
                Id = 2,
                KallenavnUFO = "JostedalsUFOen",
                TidspunktObservert = new DateTime(2021 - 10 - 05),
                KommuneObservert = "Jostedal",
                BeskrivelseAvObservasjon = "UFOen dukket ned i dalen og forsvant.",
                FornavnObservator = "Mikael",
                EtternavnObservator = "Svensen"
            };
            var observasjon3 = new Observasjon
            {
                Id = 1,
                KallenavnUFO = "TreUFOen",
                TidspunktObservert = new DateTime(2005 - 05 - 12),
                KommuneObservert = "Kongsvinger",
                BeskrivelseAvObservasjon = "Et hogstfelt med kornsirkler.",
                FornavnObservator = "Jon",
                EtternavnObservator = "Østby"

            };

            
        }
    }
}