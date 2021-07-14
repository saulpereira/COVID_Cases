using COVID_Cases.Controllers;
using COVID_Cases.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UnitTestProject
{
    [TestClass]
    public class TestUnitController
    {
        [TestMethod]
        public void TestModelIndexIsNotNull()
        {
            //Arrange
            HomeController controller = new HomeController();

            //Act
            var result = controller.Index() as ViewResult;
            var model1 = result.Model;

            //Assert
            Assert.IsNotNull(model1);
        }

        [TestMethod]
        public void TestRegionsIsNotEmpty()
        {
            //Arrange
            HomeController controller = new HomeController();

            //Act
            var result = DataApi.GetRegions();
            
            //Assert
            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public void TestCountriesIsNotEmpty()
        {
            //Arrange
            HomeController controller = new HomeController();

            //Act
            var result = DataApi.GetCountries();

            //Assert
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
