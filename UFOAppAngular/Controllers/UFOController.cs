using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UFOAppAngular.DAL;
using UFOAppAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFOAppAngular.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class UFOController : ControllerBase
    {
        private readonly IUFORepository _db;
        private ILogger<UFOController> _log;


        public UFOController(IUFORepository db, ILogger<UFOController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<bool> LagreObservasjon(Observasjon innObservasjon)
        {
            return await _db.LagreObservasjon(innObservasjon);
        }

        [Route("HentAlleObservasjoner")]
        [HttpGet]
        public async Task<ActionResult> HentAlleObservasjoner()
        {
            List<Observasjon> Observasjoner = await _db.HentAlleObservasjoner();

            return Ok(Observasjoner);

        }

        [Route("HentEnObservasjon/{id?}")]
        [HttpGet]
        public async Task<ActionResult> HentEnObservasjon(int id)
        {
            Observasjon observasjonen = await _db.HentEnObservasjon(id);
            if (observasjonen == null)
            {
                _log.LogInformation("Fant ikke observasjonen");
                return NotFound("Fant ikke observasjonen");
            }
            return Ok(observasjonen);
        }


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
    }
}
