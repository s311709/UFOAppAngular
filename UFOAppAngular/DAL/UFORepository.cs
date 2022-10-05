﻿using Microsoft.EntityFrameworkCore;
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

                //Først sjekkes det om observatør finnes fra før, dersom den ikke gjør det lagres en ny observatør
                Observator funnetObservator = await _db.Observatorer.FirstOrDefaultAsync(o => o.Etternavn == innObservasjon.EtternavnObservator && o.Fornavn == innObservasjon.FornavnObservator);

                if (funnetObservator == null)
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
                    funnetObservator = await _db.Observatorer.FirstOrDefaultAsync(o => o.Etternavn == innObservasjon.EtternavnObservator && o.Fornavn == innObservasjon.FornavnObservator);

                }

                //Deretter sjekkes det om UFO finnes før det lages en ny UFO hvis den ikke finnes

                UFO funnetUFO = await _db.UFOer.FirstOrDefaultAsync(u => u.Kallenavn == innObservasjon.KallenavnUFO);

                if (funnetUFO == null)
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
                    //hva gjør denne?
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
                await _db.SaveChangesAsync();

                //Så legges EnkeltObservasjon inn i listene til UFO og Observatør

                // funnetUFO.Observasjoner.Add(nyEnkeltObservasjonRad);
                // funnetObservatør.RegistrerteObservasjoner.Add(nyEnkeltObservasjonRad);

                //Til slutt oppdateres UFO og Observatør sine atributter antallObservasjoner og sistObservert

                funnetObservator.AntallRegistrerteObservasjoner++;
                funnetUFO.GangerObservert++;

                foreach (EnkeltObservasjon observasjon in funnetUFO.Observasjoner)
                {
                    
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > funnetUFO.SistObservert)
                    {
                        funnetUFO.SistObservert = observasjon.TidspunktObservert;
                    }
                }

                foreach (EnkeltObservasjon observasjon in funnetObservator.RegistrerteObservasjoner)
                {
                   
                    //setter SistObservert-atributten
                    if (observasjon.TidspunktObservert > funnetObservator.SisteObservasjon)
                    {
                        funnetObservator.SisteObservasjon = observasjon.TidspunktObservert;
                    }
                }
               
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
    }
}