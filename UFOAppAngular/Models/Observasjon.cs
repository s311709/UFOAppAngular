using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFOAppAngular.Models
{
    public class Observasjon
    {
        public int Id { get; set; }
        public string KallenavnUFO { get; set; }
        public DateTime TidspunktObservert { get; set; }
        public string KommuneObservert { get; set; }
        public string BeskrivelseAvObservasjon { get; set; }
        public string Modell { get; set; }
        public string FornavnObservator { get; set; }
        public string EtternavnObservator { get; set; }
        public string TelefonObservator { get; set; }
        public string EpostObservator { get; set; }
    }
}
