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
        public string FornavnObservatør { get; set; }
        public string EtternavnObservatør { get; set; }
        public string TelefonObservatør { get; set; }
        public string EpostObservatør { get; set; }
    }
}
