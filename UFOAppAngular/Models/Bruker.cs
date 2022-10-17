using System.ComponentModel.DataAnnotations;
using System;

namespace UFOAppAngular.DAL
{
    public class Bruker
    {
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public String Brukernavn { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{4,30}$")]
        public String Passord { get; set; }
    }

}