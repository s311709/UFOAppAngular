using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFOAppAngular.Models
{
    public class Observasjon
    {
        public int Id { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string KallenavnUFO { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ:. \-]{2,250}")]
        public DateTime TidspunktObservert { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string KommuneObservert { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string BeskrivelseAvObservasjon { get; set; }
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,250}")]
        public string Modell { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string FornavnObservator { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string EtternavnObservator { get; set; }
        [RegularExpression(@"[0-9]{8}")]
        public string TelefonObservator { get; set; }
        [RegularExpression(@"[0-9a-zA-ZøæåØÆÅ. \-.@ ]{2,30}")]
        public string EpostObservator { get; set; }

        public static implicit operator Observasjon(Observasjon v)
        {
            throw new NotImplementedException();
        }
    }
}
