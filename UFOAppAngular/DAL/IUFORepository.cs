using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFOAppAngular.Models;

namespace UFOAppAngular.DAL
{
    public interface IUFORepository
    {

        Task<bool> LagreObservasjon(Observasjon innObservasjon);
        Task <List<Observasjon>> HentAlleObservasjoner();
        Task<Observasjon> HentEnObservasjon(int id);
        Task<List<UFO>> HentAlleUFOer();

        Task<UFO> HentEnUFO(string kallenavn);

        Task<List<Observator>> HentAlleObservatorer();

        Task<Observator> HentEnObservator(string fornavn, string etternavn);

        Task<bool> SlettObservasjon(int id);

        Task<bool> EndreObservasjon(Observasjon endreObservasjon);

        Task<bool> LoggInn(Bruker bruker);

    }

}
