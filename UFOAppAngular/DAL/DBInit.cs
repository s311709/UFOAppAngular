using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFOAppAngular.DAL
{
    public class DBInit
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<UFOContext>();

                //sletter og oppretter databasen for seeding
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();


                var observatør1 = new Observatør { Fornavn = "Per", Etternavn = "Nydalen", Telefon = "74295105", Epost = "pnydalen@epost.no", RegistrerteObservasjoner = new List<EnkeltObservasjon>() };
                var observatør2 = new Observatør { Fornavn = "Johanne", Etternavn = "Viken", Telefon = "79376924", Epost = "jviken@epost.no", RegistrerteObservasjoner = new List<EnkeltObservasjon>() };
                var observatør3 = new Observatør { Fornavn = "Erik", Etternavn = "Hansen", Telefon = "67478474", Epost = "ehansen@epost.no", RegistrerteObservasjoner = new List<EnkeltObservasjon>() };

                var ufo1 = new UFO { Kallenavn = "Nyttårs-UFOen", Modell = "Flygende tallerken", Observasjoner = new List<EnkeltObservasjon>(), GangerObservert=0 };
                var ufo2 = new UFO { Kallenavn = "Korn-UFOen i Østfold", Modell = "Kornåker-UFO", Observasjoner = new List<EnkeltObservasjon>(), GangerObservert=0 };

                //Forklaring av datetime:
                //År, måned, dag, time, minutt, sekund
                var observasjon1 = new EnkeltObservasjon
                {
                    TidspunktObservert = new DateTime(2022, 01, 01, 0, 0, 0),
                    KommuneObservert = "Oslo",
                    BeskrivelseAvObservasjon = "UFOen fløy over Stortinget under nyttårsfeiringen",
                    Observatør = observatør1,
                    ObservertUFO = ufo1
                };

                var observasjon2 = new EnkeltObservasjon
                {
                    TidspunktObservert = new DateTime(2005, 01, 01, 0, 0, 0),
                    KommuneObservert = "Narvik",
                    BeskrivelseAvObservasjon = "UFOen kunne sees over sjøen der det ikke var raketter",
                    Observatør = observatør1,
                    ObservertUFO = ufo1
                };

                var observasjon3 = new EnkeltObservasjon
                {
                    TidspunktObservert = new DateTime(2009, 04, 21, 8, 30, 0),
                    KommuneObservert = "Halden",
                    BeskrivelseAvObservasjon = "Sirkelfenomen i åker",
                    Observatør = observatør2,
                    ObservertUFO = ufo2
                };

                var observasjon4 = new EnkeltObservasjon
                {
                    TidspunktObservert = new DateTime(2010, 9, 4, 8, 30, 0),
                    KommuneObservert = "Våler",
                    BeskrivelseAvObservasjon = "Ødelagte avlinger pga UFO-aktivitet",
                    Observatør = observatør3,
                    ObservertUFO = ufo2
                };

                //legger til observasjoner i lister
                ufo1.Observasjoner.Add(observasjon1);
                ufo1.Observasjoner.Add(observasjon2);
                ufo2.Observasjoner.Add(observasjon3);
                ufo2.Observasjoner.Add(observasjon4);

                observatør1.RegistrerteObservasjoner.Add(observasjon1);
                observatør1.RegistrerteObservasjoner.Add(observasjon2);
                observatør2.RegistrerteObservasjoner.Add(observasjon3);
                observatør3.RegistrerteObservasjoner.Add(observasjon4);

                //Legger til inkrementerings-atributter
                ufo1.SistObservert = new DateTime(); //setter først dato-tid til lavest mulige verdi for å kunne finne dato sist observert 
                ufo2.SistObservert = new DateTime(); 
                
                foreach (var observasjon in ufo1.Observasjoner)
                {
                    //setter GangerObservert-atributten vha inkrementering gjennom listen over observasjoner
                    ufo1.GangerObservert++;
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > ufo1.SistObservert)
                    {
                        ufo1.SistObservert = observasjon.TidspunktObservert;
                    } 
                }
                foreach (var observasjon in ufo2.Observasjoner)
                {
                    //setter GangerObservert-atributten vha inkrementering gjennom listen over observasjoner
                    ufo2.GangerObservert++;
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > ufo2.SistObservert)
                    {
                        ufo2.SistObservert = observasjon.TidspunktObservert;
                    }
                }


                observatør1.SisteObservasjon = new DateTime();
                observatør2.SisteObservasjon = new DateTime();
                observatør3.SisteObservasjon = new DateTime();

                foreach (var observasjon in observatør1.RegistrerteObservasjoner)
                {
                    //setter GangerObservert-atributten vha inkrementering gjennom listen over observasjoner
                    observatør1.AntallRegistrerteObservasjoner++;
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > observatør1.SisteObservasjon)
                    {
                        observatør1.SisteObservasjon = observasjon.TidspunktObservert;
                    }
                }
                foreach (var observasjon in observatør2.RegistrerteObservasjoner)
                {
                    //setter GangerObservert-atributten vha inkrementering gjennom listen over observasjoner
                    observatør2.AntallRegistrerteObservasjoner++;
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > observatør2.SisteObservasjon)
                    {
                        observatør2.SisteObservasjon = observasjon.TidspunktObservert;
                    }
                }
                foreach (var observasjon in observatør3.RegistrerteObservasjoner)
                {
                    //setter GangerObservert-atributten vha inkrementering gjennom listen over observasjoner
                    observatør3.AntallRegistrerteObservasjoner++;
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > observatør3.SisteObservasjon)
                    {
                        observatør3.SisteObservasjon = observasjon.TidspunktObservert;
                    }
                }
                context.Add(observasjon1);
                context.Add(observasjon2);
                context.Add(observasjon3);
                context.Add(observasjon4);

                context.SaveChanges();

            }
        }
    }
}
