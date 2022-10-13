using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UFOAppAngular.DAL;
using UFOAppAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UFOAppAngular.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UFOController : ControllerBase
    {
        private readonly IUFORepository _db;
        private ILogger<UFOController> _log;

        private string _ikkeLoggetInn = "";
        private const string _loggetInn = "loggetInn";
        public UFOController(IUFORepository db, ILogger<UFOController> log)
        {
            _db = db;
            _log = log;
        }

        [Route("LagreObservasjon")]
        [HttpPost]
        public async Task<ActionResult> LagreObservasjon(Observasjon innObservasjon)
        {
            bool returOK = await _db.LagreObservasjon(innObservasjon);
            if (!returOK)
            {
                _log.LogInformation("Observasjonen kunne ikke lagres!");
                return BadRequest("Observasjonen kunne ikke lagres");
            }
            return Ok("");
        }

        [Route("HentAlleObservasjoner")]
        [HttpGet]
        public async Task<ActionResult> HentAlleObservasjoner()
        {
            List<Observasjon> Observasjoner = await _db.HentAlleObservasjoner();

            return Ok(Observasjoner); //Returnerer tomt array hvis databasen er tom

        }

        [Route("HentEnObservasjon/{id?}")]
        [HttpGet]
        public async Task<ActionResult> HentEnObservasjon(int id)
        {
            Observasjon observasjonen = await _db.HentEnObservasjon(id);
            
            return Ok(observasjonen); //Denne returnerer alltid OK, returnerer en tom observasjon dersom den ikke blir funnet
        }

        [Route("HentAlleUFOer")]
        public async Task<ActionResult> HentAlleUFOer()
        {
            List<UFO> UFOer = await _db.HentAlleUFOer();

            return Ok(UFOer);

        }
        
        [Route("HentEnUFO/{kallenavn?}")]
        [HttpGet]
        public async Task<ActionResult> HentEnUFO(string kallenavn)
        {
            UFO UFO = await _db.HentEnUFO(kallenavn);
            if (UFO == null)
            {
                _log.LogInformation("Fant ikke UFOen");
                return NotFound("Fant ikke UFOen");
            }
            return Ok(UFO);
        }

        [Route("HentAlleObservatorer")]
                public async Task<ActionResult> HentAlleObservatorer()
        {
            List<Observator> Observatorer = await _db.HentAlleObservatorer();

            return Ok(Observatorer);
        }

        public async Task<ActionResult> HentEnObservator(string fornavn, string etternavn)
        {
            Observator observator = await _db.HentEnObservator(fornavn, etternavn);
            if (observator == null)
            {
                _log.LogInformation("Fant ikke observatøren");
                return NotFound("Fant ikke observatøren");
            }
            return Ok(observator);
        }

        [Route("SlettObservasjon/{id?}")]
        [HttpDelete]
        public async Task<ActionResult> SlettObservasjon(int id)
        {
            
                bool returOK = await _db.SlettObservasjon(id);
                if (!returOK)
                {
                    _log.LogInformation("Sletting av observasjon ble ikke utført");
                    return NotFound("Sletting av observasjon ble ikke utført");
                }
                return Ok();
        }
        [Route("EndreObservasjon/")]
        [HttpPut]
        public async Task<ActionResult> EndreObservasjon(Observasjon endreObservasjon)
        {
            bool returOK = await _db.EndreObservasjon(endreObservasjon);
            if (!returOK)
            {
                _log.LogInformation("Endringen kunne ikke utføres");
                return NotFound();
            }
            return Ok();
        }

        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LoggInn(bruker);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker");
                    HttpContext.Session.SetString(_loggetInn, _ikkeLoggetInn);
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, _loggetInn);
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        public void LoggUt()
        {
            HttpContext.Session.SetString(_loggetInn, "");
        }
    }
}
