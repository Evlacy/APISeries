using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIRestV1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIRestV1.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace APIRestV1.Controllers.Tests
{
    [TestClass()]
    public class SeriesControllerTests
    {
        private SeriesController controller;

        [TestInitialize]
        public void InitialisationDesTests()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("Server = localhost; port = 5432; Database = SeriesDB; uid = postgres; password = postgres;");
            SeriesDbContext _context = new SeriesDbContext(builder.Options);
            controller = new SeriesController(_context);
        }

        [TestMethod()]
        public void GetSeriesTest()
        {
            List<Serie> lesSeriesTests = new List<Serie>();
            lesSeriesTests.AddRange(new List<Serie> {
                new Serie(
                    1,
                    "Scrubs",
                    "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !",
                    9,
                    184,
                    2001,
                    "ABC (US)"),
                new Serie(
                    2,
                    "James May's 20th Century",
                    "The world in 1999 would have been unrecognisable to anyone from 1900. James May takes a look at some of the greatest developments of the 20th century, and reveals how they shaped the times we live in now.",
                    1,
                    6,
                    2007,
                    "BBC Two"),
                new Serie(
                    3,
                    "True Blood",
                    "Ayant trouvé un substitut pour se nourrir sans tuer (du sang synthétique), les vampires vivent désormais parmi les humains. Sookie, une serveuse capable de lire dans les esprits, tombe sous le charme de Bill, un mystérieux vampire. Une rencontre qui bouleverse la vie de la jeune femme...",
                    7,
                    81,
                    2008,
                    "HBO"),
            });

            var result = controller.GetSeries().Result;
            Assert.IsInstanceOfType(result, typeof(Task<ActionResult<IEnumerable<Serie>>>), "Pas un ActionResult");

            List<Serie> lesSeries = result.Value.Where(s => s.Serieid <= 3).ToList();

            CollectionAssert.AreEqual(lesSeries, lesSeriesTests, "Les series ne correspondent pas"); 
        }

        [TestMethod]
        public void GetSerie_ExistingIdPassed_ReturnsRightItem()
        {
            // Act
            var result = controller.GetSerie(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Serie>), "Pas un ActionResult");
            Assert.IsNull(result.Result, "Erreur est pas null");

            Assert.IsInstanceOfType(result.Result.Value, typeof(Serie), "Pas une Serie");
            Assert.AreEqual(new Serie(1, "Scrubs", "J.D. est un jeune médecin qui débute sa carrière dans l'hôpital du Sacré-Coeur. Il vit avec son meilleur ami Turk, qui lui est chirurgien dans le même hôpital. Très vite, Turk tombe amoureux d'une infirmière Carla. Elliot entre dans la bande. C'est une étudiante en médecine quelque peu surprenante. Le service de médecine est dirigé par l'excentrique Docteur Cox alors que l'hôpital est géré par le diabolique Docteur Kelso. A cela viennent s'ajouter plein de personnages hors du commun : Todd le chirurgien obsédé, Ted l'avocat dépressif, le concierge qui trouve toujours un moyen d'embêter JD... Une belle galerie de personnage !", 9, 184, 2001, "ABC (US)"), (Serie)result.Result.Value, "Devises pas identiques");
        }

        [TestMethod]
        public void GetSerie_UnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var result = controller.GetSerie(0);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Serie>), "Pas un ActionResult");

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.IsNull(result.Result.Value, "Pas de Serie");
        } 

        [TestMethod]
        public void Delete_NotOk_ReturnsNotFound()
        {
            // Act
            var result = controller.DeleteSerie(150);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Serie>), "Pas un ActionResult"); // Test du type de retour

            NotFoundResult notFoundResult = (NotFoundResult)result.Result;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode, "Pas un StatusCode");
        }

        [TestMethod]
        public void Delete_Ok_ReturnsRightItem()
        {
            // Act
            var result = controller.DeleteSerie(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Serie>), "Pas un ActionResult"); // Test du type de retour$

            NotFoundResult notFoundResult = (NotFoundResult)result.Result;
            Assert.AreEqual(StatusCodes.Status204NoContent, notFoundResult.StatusCode, "Pas un StatusCode");
        }

        [TestMethod]
        public void Post_ValidObjectPassed_ReturnsObject()
        {
            /*
            // Act
            Serie serie = new Serie("No", "Man's", 5, 1, 2000, "HBO");
            var result = controller.PostSerie(serie);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Serie>), "Pas un ActionResult");

            CreatedAtRouteResult routeResult = (CreatedAtRouteResult)result.Result.Result;

            Assert.AreEqual(routeResult.StatusCode, StatusCodes.Status201Created, "Pas un ActionResult");
            Assert.AreEqual(routeResult.Value, new Serie(serie.Serieid, "No", "Man's", 5, 1, 2000, "HBO"), "Pas un ActionResult");
            */
        }
    }
}