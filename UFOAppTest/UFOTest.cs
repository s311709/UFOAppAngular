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
                Id = 1,
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

        
    }
}