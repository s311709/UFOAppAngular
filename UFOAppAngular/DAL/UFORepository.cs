using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UFOAppAngular.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFOAppAngular.Models;

namespace UFOAppAngular.DAL
{
    public class UFORepository : IUFORepository
    {
        private readonly UFOContext _db;
        private ILogger<UFOController> _log;


        public UFORepository(UFOContext db, ILogger<UFOController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<bool> LagreObservasjon(Observasjon innObservasjon)
        {
            try
            {
                //Sjekkes om observatør finnes fra før
                Observator funnetObservator = await _db.Observatorer.FirstOrDefaultAsync(o => o.Etternavn == innObservasjon.EtternavnObservator && o.Fornavn == innObservasjon.FornavnObservator);

                //Observatør ikke funnet
                if (funnetObservator == null)
                {
                    //Lagrer ny observatør
                    await LagreNyObservator(innObservasjon);
                    //Henter observatør fra databasen
                    funnetObservator = await _db.Observatorer.FirstOrDefaultAsync(o => o.Etternavn == innObservasjon.EtternavnObservator && o.Fornavn == innObservasjon.FornavnObservator);
                }

                //Sjekkes om UFO finnes fra før
                UFO funnetUFO = await _db.UFOer.FirstOrDefaultAsync(u => u.Kallenavn == innObservasjon.KallenavnUFO);

                if (funnetUFO == null)
                {
                    //Lagrer ny  UFO 
                    await LagreNyUFO(innObservasjon);
                    //Henter ny UFO fra databasen
                    funnetUFO = await _db.UFOer.FirstOrDefaultAsync(u => u.Kallenavn == innObservasjon.KallenavnUFO);
                }

                //Deretter lages en ny EnkeltObservasjon med UFOen og Observatøren i attributter inni EnkeltObservasjon
                EnkeltObservasjon nyEnkeltObservasjonRad = new EnkeltObservasjon
                {
                    TidspunktObservert = innObservasjon.TidspunktObservert,
                    KommuneObservert = innObservasjon.KommuneObservert,
                    BeskrivelseAvObservasjon = innObservasjon.BeskrivelseAvObservasjon,
                    ObservertUFO = funnetUFO,
                    Observator = funnetObservator
                };

                _db.EnkeltObservasjoner.Add(nyEnkeltObservasjonRad);

                //Enkeltobservasjonen legges til UFO og Observatør
                funnetUFO.Observasjoner.Add(nyEnkeltObservasjonRad);
                funnetObservator.RegistrerteObservasjoner.Add(nyEnkeltObservasjonRad);

                //Til slutt oppdateres UFO og Observatør sine atributter antallObservasjoner og sistObservert
                funnetObservator.AntallRegistrerteObservasjoner++;
                funnetUFO.GangerObservert++;

                //Siste observasjon oppdateres
                await OppdaterSisteObservasjon(funnetUFO, funnetObservator);

                await _db.SaveChangesAsync();

                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Observasjon>> HentAlleObservasjoner()
        {
            try
            {
                List<EnkeltObservasjon> alleEnkeltObservasjoner = await _db.EnkeltObservasjoner.ToListAsync();

                List<Observasjon> alleObservasjoner = new List<Observasjon>();

                foreach (var enkeltObservasjon in alleEnkeltObservasjoner)
                {

                    var enObservasjon = new Observasjon
                    {
                        Id = enkeltObservasjon.Id,
                        KallenavnUFO = enkeltObservasjon.ObservertUFO.Kallenavn,
                        TidspunktObservert = enkeltObservasjon.TidspunktObservert,
                        KommuneObservert = enkeltObservasjon.KommuneObservert,
                        BeskrivelseAvObservasjon = enkeltObservasjon.BeskrivelseAvObservasjon,
                        FornavnObservator = enkeltObservasjon.Observator.Fornavn,
                        EtternavnObservator = enkeltObservasjon.Observator.Etternavn
                    };
                    alleObservasjoner.Add(enObservasjon);
                }
                return alleObservasjoner;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<Observasjon> HentEnObservasjon(int id)
        {
            try
            {
                EnkeltObservasjon enkeltObservasjon = await _db.EnkeltObservasjoner.FindAsync(id);

                var enObservasjon = new Observasjon
                {
                    Id = enkeltObservasjon.Id,
                    KallenavnUFO = enkeltObservasjon.ObservertUFO.Kallenavn,
                    TidspunktObservert = enkeltObservasjon.TidspunktObservert,
                    KommuneObservert = enkeltObservasjon.KommuneObservert,
                    BeskrivelseAvObservasjon = enkeltObservasjon.BeskrivelseAvObservasjon,
                    Modell = enkeltObservasjon.ObservertUFO.Modell,
                    FornavnObservator = enkeltObservasjon.Observator.Fornavn,
                    EtternavnObservator = enkeltObservasjon.Observator.Etternavn,
                    TelefonObservator = enkeltObservasjon.Observator.Telefon,
                    EpostObservator = enkeltObservasjon.Observator.Epost
                };
                return enObservasjon;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }

        }


        public async Task<List<UFO>> HentAlleUFOer()
        {
            List<UFO> alleUFOer = await _db.UFOer.ToListAsync();

            List<UFO> returUFOer = new List<UFO>();

            foreach (var UFO in alleUFOer)
            {
                var returUFO = new UFO
                {
                    Id = UFO.Id,
                    Kallenavn = UFO.Kallenavn,
                    Modell = UFO.Modell,
                    SistObservert = UFO.SistObservert,
                    GangerObservert = UFO.GangerObservert
                };
                returUFOer.Add(returUFO);
            }

            return returUFOer;
        }

        public async Task<UFO> HentEnUFO(string kallenavn)
        {
            try
            {
                UFO funnetUFO = await _db.UFOer.FirstOrDefaultAsync(u => u.Kallenavn == kallenavn);

                var returUFO = new UFO
                {
                    Id = funnetUFO.Id,
                    Kallenavn = funnetUFO.Kallenavn,
                    Modell = funnetUFO.Modell,
                    SistObservert = funnetUFO.SistObservert,
                    GangerObservert = funnetUFO.GangerObservert
                };

                return returUFO;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }

        }

        public async Task<List<Observator>> HentAlleObservatorer()
        {
            try
            {
                List<Observator> alleObervatorer = await _db.Observatorer.ToListAsync();

                List<Observator> returObservatorer = new List<Observator>();

                foreach (var enkeltObservator in alleObervatorer)
                {
                    var enObservator = new Observator
                    {
                        Id = enkeltObservator.Id,
                        Fornavn = enkeltObservator.Fornavn,
                        Etternavn = enkeltObservator.Etternavn,
                        Telefon = enkeltObservator.Telefon,
                        Epost = enkeltObservator.Epost,
                        AntallRegistrerteObservasjoner = enkeltObservator.AntallRegistrerteObservasjoner,
                        SisteObservasjon = enkeltObservator.SisteObservasjon
                    };
                    returObservatorer.Add(enObservator);
                }
                return returObservatorer;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<Observator> HentEnObservator(string fornavn, string etternavn)
        {
            try
            {
                Observator funnetObservator = await _db.Observatorer.FirstOrDefaultAsync(o => o.Etternavn == etternavn && o.Fornavn == fornavn);

                var returObservator = new Observator
                {
                    Id = funnetObservator.Id,
                    Fornavn = funnetObservator.Fornavn,
                    Etternavn = funnetObservator.Etternavn,
                    Telefon = funnetObservator.Telefon,
                    Epost = funnetObservator.Epost,
                    AntallRegistrerteObservasjoner = funnetObservator.AntallRegistrerteObservasjoner,
                    SisteObservasjon = funnetObservator.SisteObservasjon
                };

                return returObservator;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }

        }

        public async Task<bool> SlettObservasjon(int id)
        {
            try
            {
                //Finner observasjon som skal slettes
                EnkeltObservasjon enkeltObservasjon = await _db.EnkeltObservasjoner.FindAsync(id);

                //Finner tilknyttet UFO og observatør
                UFO enkeltObservasjonUFO = await _db.UFOer.FirstOrDefaultAsync(u => u.Id == enkeltObservasjon.ObservertUFO.Id);
                Observator enkeltObservasjonObservator = await _db.Observatorer.FirstOrDefaultAsync(o => o.Id == enkeltObservasjon.Observator.Id);

                //Fjerner observasjonen fra listene
                enkeltObservasjonUFO.Observasjoner.Remove(enkeltObservasjon);
                enkeltObservasjonObservator.RegistrerteObservasjoner.Remove(enkeltObservasjon);

                //Dekrementerer antall ganger observert teller
                enkeltObservasjonUFO.GangerObservert--;
                enkeltObservasjonObservator.AntallRegistrerteObservasjoner--;

                //Sist observert resettes
                await OppdaterSisteObservasjon(enkeltObservasjonUFO, enkeltObservasjonObservator);

                //Enkeltobservasjon fjernes fra databasen
                _db.EnkeltObservasjoner.Remove(enkeltObservasjon);

                //Endringer i databasen lagres
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        //EndreObservasjon
        public async Task<bool> EndreObservasjon(Observasjon endreObservasjon)
        {
            try
            {
                var endreObjekt = await _db.EnkeltObservasjoner.FindAsync(endreObservasjon.Id);

                //enkeltObservasjon
                endreObjekt.TidspunktObservert = endreObservasjon.TidspunktObservert;
                endreObjekt.KommuneObservert = endreObservasjon.KommuneObservert;
                endreObjekt.BeskrivelseAvObservasjon = endreObservasjon.BeskrivelseAvObservasjon;

                //UFO
                endreObjekt.ObservertUFO.Kallenavn = endreObservasjon.KallenavnUFO;
                endreObjekt.ObservertUFO.Modell = endreObservasjon.Modell;

                //Observatør
                endreObjekt.Observator.Fornavn = endreObservasjon.FornavnObservator;
                endreObjekt.Observator.Etternavn = endreObservasjon.EtternavnObservator;
                endreObjekt.Observator.Telefon = endreObservasjon.TelefonObservator;
                endreObjekt.Observator.Epost = endreObservasjon.EpostObservator;

                //Sist observert resettes
                await OppdaterSisteObservasjon(endreObjekt.ObservertUFO, endreObjekt.Observator);

                await _db.SaveChangesAsync();

            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> LagreNyUFO(Observasjon innObservasjon)
        {
            try
            {
                var nyUFOrad = new UFO
                {
                    Modell = innObservasjon.Modell,
                    Kallenavn = innObservasjon.KallenavnUFO,
                    Observasjoner = new List<EnkeltObservasjon>(),
                    GangerObservert = 0,
                    SistObservert = new DateTime()
                };
                _db.UFOer.Add(nyUFOrad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> LagreNyObservator(Observasjon innObservasjon)
        {
            try
            {
                var nyObservatorrad = new Observator
                {
                    Fornavn = innObservasjon.FornavnObservator,
                    Etternavn = innObservasjon.EtternavnObservator,
                    Telefon = innObservasjon.TelefonObservator,
                    Epost = innObservasjon.EpostObservator,
                    RegistrerteObservasjoner = new List<EnkeltObservasjon>(),
                    AntallRegistrerteObservasjoner = 0,
                    SisteObservasjon = new DateTime()
                };
                _db.Observatorer.Add(nyObservatorrad);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> OppdaterSisteObservasjon(UFO innUFO, Observator innObservator)
        {
            try
            {
                innUFO.SistObservert = new DateTime();
                innObservator.SisteObservasjon = new DateTime();

                foreach (EnkeltObservasjon observasjon in innUFO.Observasjoner)
                {

                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > innUFO.SistObservert)
                    {
                        innUFO.SistObservert = observasjon.TidspunktObservert;
                    }
                }

                foreach (EnkeltObservasjon observasjon in innObservator.RegistrerteObservasjoner)
                {

                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > innObservator.SisteObservasjon)
                    {
                        innObservator.SisteObservasjon = observasjon.TidspunktObservert;
                    }
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

    }

}
