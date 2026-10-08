using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static System.Net.WebRequestMethods;

namespace TARge25Shop.SeleniumTesting
{
    public class SpaceShipFrontendTest
    {
        [Fact]
        public void Should_NavigateToCreate_AddSpaceshipWithCorrectData_ReturnToIndex()
        {
            IWebDriver driver = SetupAndNavigateSindex();
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();
            //sistetavad andmet
            InsertSpaceShipData(driver, false);
            Thread.Sleep(500);
            

            IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_CreateSpaceShip"));
            cu_CreateSpaceship.Click();
            Thread.Sleep(1000);

            IWebElement IndexNameSpaceship = driver.FindElement(By.Id("IndexNameSpaceship"));
            var spaceShipNameData = IndexNameSpaceship.Text;
            IWebElement IndexTypeSpaceship = driver.FindElement(By.Id("IndexTypeSpaceship"));
            var spaceTypeNameData = IndexTypeSpaceship.Text;
            IWebElement IndexCrewSpaceship = driver.FindElement(By.Id("IndexCrewSpaceship"));
            var spaceShipCrewData = IndexCrewSpaceship.Text;

            Assert.Equal(spaceShipNameData, "i add name for spachop");
            Assert.Equal(spaceTypeNameData, "i add type for spachop");
            Assert.Equal(spaceShipCrewData, "1234");
        }

        private static IWebDriver SetupAndNavigateSindex()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Url = "https://localhost:7227/";
            IWebElement navigatetoSpaceship = driver.FindElement(By.LinkText("Spaceship"));
            navigatetoSpaceship.Click();
            

            return driver;
        }
        [Fact]
        public void Should_NavigateToUpdate_OfASpaceShip_WithNewdata()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSindex();
            //IWebElement cu_UpdateSpaceship = driver.FindElement(By.Id("IndexSpaceshipUpdate"));
            //cu_UpdateSpaceship.Click();
            //IndexSpaceshipUpdate
            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == "i add name for spachop")
                {
                    //        //siis vajuta selles reas asuvat details nuppu
                    //        row.FindElement(By.Id("IndexSpaceshipDetails")).Click();
                    var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                    var button = rowelement.FindElement(By.Id("IndexSpaceshipUpdate"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }
            InsertSpaceShipData(driver, true);
            Thread.Sleep(500);


            IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_UpdateSpaceShip"));
            cu_CreateSpaceship.Click();
            Thread.Sleep(1000);

            IWebElement IndexNameSpaceship = driver.FindElement(By.Id("IndexNameSpaceship"));
            var spaceShipNameData = IndexNameSpaceship.Text;
            IWebElement IndexTypeSpaceship = driver.FindElement(By.Id("IndexTypeSpaceship"));
            var spaceTypeNameData = IndexTypeSpaceship.Text;
            IWebElement IndexCrewSpaceship = driver.FindElement(By.Id("IndexCrewSpaceship"));
            var spaceShipCrewData = IndexCrewSpaceship.Text;

            Assert.Equal(spaceShipNameData, "2i add name for spachop2");
            Assert.Equal(spaceTypeNameData, "2i add type for spachop2");
            Assert.Equal(spaceShipCrewData, "212342");

        }

        [Fact]
        public void Should_NavigateToDetails_OfASpaceShip_WithPreviouslyCorrectData_AndReturnToIndex()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSindex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == "i add name for spachop")
                {
                    //        //siis vajuta selles reas asuvat details nuppu
                    //        row.FindElement(By.Id("IndexSpaceshipDetails")).Click();
                    var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
                    var button = rowelement.FindElement(By.Id("IndexSpaceshipDetails"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }
            //InsertSpaceShipData(driver, true);
            Thread.Sleep(500);
            //andmete kogumine pärast reloadi
            IWebElement details_SpaceshipId = driver.FindElement(By.Id("id"));
            var details_id = details_SpaceshipId.Text;

            IWebElement details_SpaceshipName = driver.FindElement(By.Id("name"));
            var details_name = details_SpaceshipName.Text;

            IWebElement details_SpaceshipType = driver.FindElement(By.Id("type"));
            var details_type = details_SpaceshipType.Text;

            IWebElement details_SpaceshipCrew = driver.FindElement(By.Id("crew"));
            var details_crew = details_SpaceshipCrew.Text;

            IWebElement details_SpaceshipPower = driver.FindElement(By.Id("power"));
            var details_power = details_SpaceshipPower.Text;
            //kontroll

            Assert.NotNull(details_id);
            Assert.True(details_id.Contains("-") && (details_id.Count('-') == 4));
            Assert.True(details_id.Substring(0,9).EndsWith('-'));
            Assert.Equal("i add name for spachop", details_name);
            //Assert.Equal("i add type for spachop", details_ShipType);
            Assert.Equal("1234", details_crew);
            Assert.Equal("65467", details_power);
            

            //details_spaceshipId.Click();

        }
        private void InsertSpaceShipData(IWebDriver driver, bool newdata)
        {
            if (newdata == false)
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceShip"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys("i add name for spachop");
                IWebElement cu_ShipTypeSpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceShip"));
                cu_ShipTypeSpaceship.Clear();
                cu_ShipTypeSpaceship.SendKeys("i add type for spachop");
                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceShip"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("1234");
                IWebElement cu_EnginePowerSpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceShip"));
                cu_EnginePowerSpaceship.Clear();
                cu_EnginePowerSpaceship.SendKeys("65467");
            }
            else
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceShip"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys("2i add name for spachop2");
                IWebElement cu_ShipTypeSpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceShip"));
                cu_ShipTypeSpaceship.Clear();
                cu_ShipTypeSpaceship.SendKeys("2i add type for spachop2");
                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceShip"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("212342");
                IWebElement cu_EnginePowerSpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceShip"));
                cu_EnginePowerSpaceship.Clear();
                cu_EnginePowerSpaceship.SendKeys("2654672");

            }
        }
    }
    }

