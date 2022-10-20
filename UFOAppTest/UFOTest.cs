using System;
using Xunit;
using Moq; 
using UFOAppAngular.Controllers; // må legge til en prosjektreferanse i Project-> Add Reference -> Project
using UFOAppAngular.DAL;
using System.Threading.Tasks;
using System.Collections.Generic;
using UFOAppAngular.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UFOAppTest
{
    public class UFOControllerTest
    {

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IUFORepository> mockRep = new Mock<IUFORepository>();
        private readonly Mock<ILogger<UFOController>> mockLog = new Mock<ILogger<UFOController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task HentAlleObservasjonerLoggetInnOK()
        {
            //Arrange
            var observasjon1 = new Observasjon
            {
                Id = 1,
                KallenavnUFO = "KornUFOen i Stavanger",
                TidspunktObservert = new DateTime(2022 - 01 - 01),
                KommuneObservert = "Stavanger",
                BeskrivelseAvObservasjon = "UFOen fløy i sirkler over byen.",
                FornavnObservator = "Heidi",
                EtternavnObservator = "Lyngås"
            };
            var observasjon2 = new Observasjon
            {
                Id = 2,
                KallenavnUFO = "JostedalsUFOen",
                TidspunktObservert = new DateTime(2021 - 10 - 05),
                KommuneObservert = "Jostedal",
                BeskrivelseAvObservasjon = "UFOen dukket ned i dalen og forsvant.",
                FornavnObservator = "Mikael",
                EtternavnObservator = "Svensen"
            };
            var observasjon3 = new Observasjon
            {
                Id = 3,
                KallenavnUFO = "TreUFOen",
                TidspunktObservert = new DateTime(2005 - 05 - 12),
                KommuneObservert = "Kongsvinger",
                BeskrivelseAvObservasjon = "Et hogstfelt med kornsirkler.",
                FornavnObservator = "Jon",
                EtternavnObservator = "Østby"

            };

            var observasjonsListe = new List<Observasjon>();
            observasjonsListe.Add(observasjon1);
            observasjonsListe.Add(observasjon2);
            observasjonsListe.Add(observasjon3);

            mockRep.Setup(o => o.HentAlleObservasjoner()).ReturnsAsync(observasjonsListe);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await UFOController.HentAlleObservasjoner() as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Observasjon>>((List<Observasjon>)resultat.Value, observasjonsListe);

        }

        [Fact]
        public async Task HentAlleObservasjonerIkkeLoggetInn()
        {

            // Arrange
            mockRep.Setup(o => o.HentAlleObservasjoner()).ReturnsAsync(It.IsAny<List<Observasjon>>());

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentAlleObservasjoner() as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task LagreObservasjonLoggetInnOK()
        {
            // Arrange
            mockRep.Setup(o => o.LagreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LagreObservasjon(It.IsAny<Observasjon>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }

        [Fact]
        public async Task LagreObservasjonLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(o => o.LagreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(false);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LagreObservasjon(It.IsAny<Observasjon>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Observasjonen kunne ikke lagres", resultat.Value);
        }

        [Fact]
        public async Task LagreObservasjonLoggetInnFeilModel()
        {


            mockRep.Setup(o => o.LagreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            UFOController.ModelState.AddModelError("KallenavnUFO", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LagreObservasjon(It.IsAny<Observasjon>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task LagreObservasjonIkkeLoggetInn()
        {
            mockRep.Setup(o => o.LagreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LagreObservasjon(It.IsAny<Observasjon>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task SlettObservasjonLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(o => o.SlettObservasjon(It.IsAny<int>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.SlettObservasjon(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }

        [Fact]
        public async Task SlettObservasjonLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(o => o.SlettObservasjon(It.IsAny<int>())).ReturnsAsync(false);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.SlettObservasjon(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Sletting av observasjon ble ikke utført", resultat.Value);
        }

        [Fact]
        public async Task SlettObservasjonIkkeLoggetInn()
        {
            mockRep.Setup(o => o.SlettObservasjon(It.IsAny<int>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.SlettObservasjon(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentEnObservasjonLoggetInnOK()
        {
            // Arrange
            var observasjon1 = new Observasjon
            {
                Id = 1,
                KallenavnUFO = "KornUFOen i Stavanger",
                TidspunktObservert = new DateTime(2022 - 01 - 01),
                KommuneObservert = "Stavanger",
                BeskrivelseAvObservasjon = "UFOen fløy i sirkler over byen.",
                FornavnObservator = "Heidi",
                EtternavnObservator = "Lyngås"
            };

            mockRep.Setup(o => o.HentEnObservasjon(It.IsAny<int>())).ReturnsAsync(observasjon1);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnObservasjon(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Observasjon>(observasjon1, (Observasjon)resultat.Value);
        }


        //public async Task HentEnObservasjonLoggetInnIkkeOK()
        //Har ikke med denne da HentEnObservasjon alltid returnerer OK

        [Fact]
        public async Task HentEnObservasjonIkkeLoggetInn()
        {
            mockRep.Setup(o => o.HentEnObservasjon(It.IsAny<int>())).ReturnsAsync(() => null);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnObservasjon(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task EndreObservasjonLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(o => o.EndreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.EndreObservasjon(It.IsAny<Observasjon>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
        }

        [Fact]
        public async Task EndreObservasjonLoggetInnIkkeOK()
        {
            // Arrange
            mockRep.Setup(o => o.EndreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(false);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.EndreObservasjon(It.IsAny<Observasjon>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Endringen av observasjonen kunne ikke utføres", resultat.Value);
        }

        [Fact]
        public async Task EndreObservasjonLoggetInnFeilModel()
        {

            var observasjon1 = new Observasjon
            {
                Id = 1,
                KallenavnUFO = "",
                TidspunktObservert = new DateTime(2022 - 01 - 01),
                KommuneObservert = "Stavanger",
                BeskrivelseAvObservasjon = "UFOen fløy i sirkler over byen.",
                FornavnObservator = "Heidi",
                EtternavnObservator = "Lyngås"
            };

            mockRep.Setup(o => o.EndreObservasjon(observasjon1)).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            UFOController.ModelState.AddModelError("KallenavnUFO", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.EndreObservasjon(observasjon1) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreObservasjonIkkeLoggetInn()
        {
            mockRep.Setup(o => o.EndreObservasjon(It.IsAny<Observasjon>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.EndreObservasjon(It.IsAny<Observasjon>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task LoggInnOK()
        {
            mockRep.Setup(o => o.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            mockRep.Setup(o => o.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.False((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnInputFeil()
        {
            mockRep.Setup(o => o.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            UFOController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }
        [Fact]
        public void LoggUt()
        {
            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            UFOController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }

        [Fact]
        public async Task HentEnObservatorLoggetInnOK()
        {
            // Arrange
            var observator1 = new Observator
            {
                Id = 1,
                Fornavn = "Heidi",
                Etternavn = "Lyngås",
                Telefon = "Stavanger",
                Epost = "h@lyngås.com",
                AntallRegistrerteObservasjoner = 1,
                SisteObservasjon = new DateTime(2022 - 01 - 01)
            };

            mockRep.Setup(o => o.HentEnObservator(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(observator1);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnObservator(It.IsAny<string>(), It.IsAny<string>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Observator>(observator1, (Observator)resultat.Value);
        }

        [Fact]
        public async Task HentEnObservatorLoggetInnIkkeOK(){
            // Arrange

            mockRep.Setup(o => o.HentEnObservator(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null); 

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnObservator(It.IsAny<string>(), It.IsAny<string>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke observatøren", resultat.Value);
        }

        [Fact]
        public async Task HentEnObservatorIkkeLoggetInn()
        {
            mockRep.Setup(o => o.HentEnObservator(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnObservator(It.IsAny<string>(), It.IsAny<string>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentEnOUFOLoggetInnOK()
        {
            // Arrange
            var ufo1 = new UFO
            {
                Id = 1,
                Kallenavn = "KornUFOen",
                Modell = "RacerUFO",
                SistObservert = new DateTime(2022 - 01 - 01),
                GangerObservert = 2
            };

            mockRep.Setup(o => o.HentEnUFO(It.IsAny<string>())).ReturnsAsync(ufo1);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnUFO(It.IsAny<string>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<UFO>(ufo1, (UFO)resultat.Value);
        }

        [Fact]
        public async Task HentEnUFOLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(o => o.HentEnUFO(It.IsAny<string>())).ReturnsAsync(() => null);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnUFO(It.IsAny<string>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke UFOen", resultat.Value);
        }

        [Fact]
        public async Task HentEnUFOIkkeLoggetInn()
        {
            mockRep.Setup(o => o.HentEnUFO(It.IsAny<string>())).ReturnsAsync(() => null);

            var UFOController = new UFOController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            UFOController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await UFOController.HentEnUFO(It.IsAny<string>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

    }
}