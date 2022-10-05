using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UFOAppAngular.DAL
{
    public class EnkeltObservasjon
    {
        [Key]
        public int Id { get; set; }
        public DateTime TidspunktObservert { get; set; } //Dato+klokkeslett
        public string KommuneObservert { get; set; }
        public string BeskrivelseAvObservasjon { get; set; }
        virtual public Observator Observator { get; set; }
        virtual public UFO ObservertUFO { get; set; }
    }

    public class UFO
    {
        [Key]
        public int Id { get; set; }
        public String Kallenavn { get; set; }
        public string Modell { get; set; }
        public virtual List<EnkeltObservasjon> Observasjoner { get; set; }
        //GangerObservert kan ha en counter i lagre/endre/slett som inkrementerer/dekrementerer for hver gang det legges inn/fjernes en observasjon i Observasjoner-listen
        public int GangerObservert { get; set; }
        //SistObservert: For lagre/endre/slett: Er det mulig å iterere gjennom Observasjoner-listen og velge den største dato-tiden blant observasjonene for å få denne atributten?
        public DateTime SistObservert { get; set; }
    }

    public class Observator
    {
        [Key]
        public int Id { get; set; }
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Telefon { get; set; }
        public string Epost {get; set;}

        public virtual List<EnkeltObservasjon> RegistrerteObservasjoner { get; set; }

        //AntallRegistrerteObservasjoner kan ha en counter i lagre/endre/slett som inkrementerer/dekrementerer for hver gang det legges inn/fjernes en observasjon i RegistrerteObservasjoner-listen
        public int AntallRegistrerteObservasjoner { get; set; }
        //SisteObservasjon: For lagre/endre/slett: Er det mulig å iterere gjennom RegistrerteObservasjoner-listen og velge den største dato-tiden blant observasjonene for å få denne atributten?
        public DateTime SisteObservasjon { get; set; }

    }

    public class UFOContext : DbContext
    {
        //oppretter databasen
        public UFOContext(DbContextOptions<UFOContext> options) :
            base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UFO> UFOer { get; set; }
        public DbSet<EnkeltObservasjon> EnkeltObservasjoner { get; set; }
        public DbSet<Observator> Observatorer { get; set; }
        
        //Denne gjør det mulig å bruke lazy loading til atributtene som er virtual
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        optionsBuilder.UseLazyLoadingProxies();
        }

    }
    
}
