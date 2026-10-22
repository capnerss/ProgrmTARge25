using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;


namespace TARge25Shop.SeleniumTesting
{
    [TestCaseOrderer("TARge25Shop.SeleniumTesting.PriorityOrderer", "TARge25Shop.SeleniumTesting")]
    public class KindergartenFrontendTests
    {
        public static string thisRunGuid { get; set; }


        [Fact]
        [Order(9)]
        public void Should_NavigateToCreate_AddUniqueKindergarten_WithCorrectData_ReturnToIndex()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            //sisestatavad andmed, siin tagastub ka kasutatud nimi
            string concreteName = InsertKindergartenData_ReturnName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_CreateKindergarten"));
            cu_CreateKindergarten.Click();

            ////arvuti tudub
            //Thread.Sleep(1000);

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string KindergartenNameData = "";
            string KindergartenTypeData = "";
            string KindergartenCrewData = "";
            string KindergartenBuiltAtData = "";
            //IWebElement indexNameKindergarten = driver.FindElement(By.Id("IndexNameKindergarten"));
            //var KindergartenNameData = indexNameKindergarten.Text;
            //IWebElement indexTypeKindergarten = driver.FindElement(By.Id("IndexTypeKindergarten"));
            //var KindergartenTypeData = indexTypeKindergarten.Text;
            //IWebElement indexCrewKindergarten = driver.FindElement(By.Id("IndexCrewKindergarten"));
            //var KindergartenCrewData = indexCrewKindergarten.Text;
            bool doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    KindergartenNameData = thisRunGuid;
                    KindergartenTypeData = row.FindElement(By.Id("IndexTypeKindergarten")).Text;
                    KindergartenCrewData = row.FindElement(By.Id("IndexCrewKindergarten")).Text;
                    KindergartenBuiltAtData = row.FindElement(By.Id("IndexCreatedAtKindergarten")).Text;
                    break;
                }
            }
            driver.Quit();
            

            //kontroll
            //Assert.True(concreteName == indexNameKindergarten.Text);
            Assert.True(doesItExist);
            Assert.Equal(thisRunGuid, KindergartenNameData);
            Assert.Equal("10", KindergartenTypeData);
            Assert.Equal("666667", KindergartenCrewData);
            Assert.Equal("12345", KindergartenBuiltAtData);
            Assert.True((thisRunGuid == KindergartenNameData)
                && ("10" == KindergartenTypeData)
                && ("666667" == KindergartenCrewData)
                && ("12345" == KindergartenBuiltAtData));
        }


        [Fact]
        [Order(10)]
        public void Should_NavigateToDetails_OfThisKindergarten_WithPreviouslyCorrectData()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isObjectFound = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid)
                {
                    isObjectFound = true;
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDetails"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }

            if (!isObjectFound)
            {
                driver.Quit();
                Assert.True(isObjectFound);
                return;
            }

            //andmete kogumine pärast reloadi
            IWebElement details_KindergartenId = driver.FindElement(By.Id("id"));
            var details_id = details_KindergartenId.Text;

            IWebElement details_KindergartenName = driver.FindElement(By.Id("name"));
            var details_name = details_KindergartenName.Text;

            IWebElement details_KindergartenType = driver.FindElement(By.Id("type"));
            var details_type = details_KindergartenType.Text;

            IWebElement details_KindergartenCrew = driver.FindElement(By.Id("crew"));
            var details_crew = details_KindergartenCrew.Text;

            IWebElement details_KindergartenPower = driver.FindElement(By.Id("power"));
            var details_power = details_KindergartenPower.Text;

            driver.Quit();
            
            //kontroll
            Assert.True(isObjectFound);
            Assert.NotNull(details_id);
            // et ei oleks tühi kontrollid kõikidele andmetele
            Assert.NotEmpty(details_name);
            Assert.NotNull(details_type);
            Assert.False(string.IsNullOrEmpty(details_power));
            Assert.True(!string.IsNullOrEmpty(details_crew));
            Assert.Equal(thisRunGuid, details_name);
            Assert.Equal("10", details_type);
            Assert.Equal("12345", details_crew);
            Assert.Equal("666667", details_power);
            Assert.True(int.Parse(details_crew).GetType() == typeof(int));
            Assert.True(int.Parse(details_power).GetType() == typeof(int));
        }


        [Fact]
        [Order(11)]
        public void Should_UpdateDetails_OfThisKindergarten_WithNewData()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenUpdate"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }

            if (!doesItExist)
            {
                driver.Quit();
                Assert.True(doesItExist);
                return;
            }

            //sisestatavad andmed
            InsertKindergartenData(driver, thisRunGuid, true);

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_UpdateKindergarten"));
            cu_CreateKindergarten.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string KindergartenNameData = "";
            string KindergartenTypeData = "";
            string KindergartenCrewData = "";
            string KindergartenBuiltAtData = "";
            doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    KindergartenNameData = thisRunGuid;
                    KindergartenTypeData = row.FindElement(By.Id("IndexTypeKindergarten")).Text;
                    KindergartenCrewData = row.FindElement(By.Id("IndexCrewKindergarten")).Text;
                    KindergartenBuiltAtData = row.FindElement(By.Id("IndexCreatedAtKindergarten")).Text;
                    break;
                }
            }
            driver.Quit();
           

            ////arvuti tudub
            //Thread.Sleep(1000);
            Assert.True(doesItExist);
            Assert.NotNull(KindergartenNameData);
            Assert.Equal(thisRunGuid, KindergartenNameData);
            Assert.False(string.IsNullOrEmpty(KindergartenTypeData));
            Assert.NotEqual("10", KindergartenTypeData);
            Assert.Equal("40", KindergartenTypeData);
            Assert.Equal("444", KindergartenCrewData);
            Assert.Equal("67890", KindergartenBuiltAtData);
        }


        [Fact]
        [Order(12)]
        public void Should_DeleteThisKindergarten_FromAppVIADeleteView()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDelete"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }

            if (!doesItExist)
            {
                driver.Quit();
                Assert.True(doesItExist);
                return;
            }

            var clickthisbutton = driver.FindElement(By.Id("Delete_Delete"));
            clickthisbutton.Click();
            Thread.Sleep(500);

            bool isFoundAfterDelete = false;
            table = driver.FindElement(By.Id("IndexTable"));
            rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            foreach (var row in rows)
            {
                var nameElement = row.FindElement(By.Id("IndexNameKindergarten")).Text;
                if (nameElement == thisRunGuid)
                {
                    isFoundAfterDelete = true;
                    break;
                }
            }
            driver.Quit();

            Assert.False(isFoundAfterDelete);
        }


        [Fact]
        [Order(13)]
        public void Should_CancelCreate_WhenClickingBack_ReturnToIndexWithoutSaving()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            string testCancelGuid = Guid.NewGuid().ToString();
            IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
            cu_NameEntryKindergarten.SendKeys(testCancelGuid);

            IWebElement backButton = driver.FindElement(By.Id("CU_Back"));
            backButton.Click();
            Thread.Sleep(500);

            //kontroll
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isFound = false;
            foreach (var row in rows)
            {
                var nameElement = row.FindElement(By.Id("IndexNameKindergarten")).Text;
                if (nameElement == testCancelGuid)
                {
                    isFound = true;
                    break;
                }
            }
            driver.Quit();

            Assert.False(isFound);
        }


        [Fact]
        [Order(14)]
        public void Should_CreateKindergarten_WithZeroChildren_ApplyDefaultValueFour()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            string testGuid = Guid.NewGuid().ToString();
            IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
            cu_NameEntryKindergarten.SendKeys(testGuid);

            IWebElement cu_ShipTypeEntryKindergarten = driver.FindElement(By.Id("CU_ShipTypeEntryKindergarten"));
            cu_ShipTypeEntryKindergarten.Clear();
            cu_ShipTypeEntryKindergarten.SendKeys("0");

            IWebElement cu_CrewEntryKindergarten = driver.FindElement(By.Id("CU_CrewEntryKindergarten"));
            cu_CrewEntryKindergarten.Clear();
            cu_CrewEntryKindergarten.SendKeys("Test");

            IWebElement cu_EnginePowerEntryKindergarten = driver.FindElement(By.Id("CU_EnginePowerEntryKindergarten"));
            cu_EnginePowerEntryKindergarten.Clear();
            cu_EnginePowerEntryKindergarten.SendKeys("Test");

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_CreateKindergarten"));
            cu_CreateKindergarten.Click();
            Thread.Sleep(500);

            //kontroll
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);

            string actualChildrenCount = "";
            bool isFound = false;
            foreach (var row in rows)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == testGuid)
                {
                    isFound = true;
                    actualChildrenCount = row.FindElement(By.Id("IndexTypeKindergarten")).Text;
                    break;
                }
            }
            driver.Quit();

            Assert.True(isFound);
            Assert.Equal("4", actualChildrenCount);
        }


        private static IWebDriver SetupAndNavigateSIndex()
        {
            //firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();
            //aadress millele draiver navigeerib
            driver.Url = "https://localhost:7227/";
            //lehelt otsitav element
            IWebElement navigateToKindergarten = driver.FindElement(By.LinkText("Kindergarten"));
            //selle elemendiga tehtav tegevus
            navigateToKindergarten.Click();
            return driver;
        }


        private void InsertKindergartenData(IWebDriver driver, string name = null, bool isOneOrTwo = false)
        {

            if (isOneOrTwo)
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                if (name != null)
                {
                    cu_NameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_NameEntryKindergarten.SendKeys("DISISENJUNAEM");
                }
                IWebElement cu_ShipTypeEntryKindergarten = driver.FindElement(By.Id("CU_ShipTypeEntryKindergarten"));
                cu_ShipTypeEntryKindergarten.Clear();
                cu_ShipTypeEntryKindergarten.SendKeys("40");
                IWebElement cu_CrewEntryKindergarten = driver.FindElement(By.Id("CU_CrewEntryKindergarten"));
                cu_CrewEntryKindergarten.Clear();
                cu_CrewEntryKindergarten.SendKeys("67890");
                IWebElement cu_EnginePowerEntryKindergarten = driver.FindElement(By.Id("CU_EnginePowerEntryKindergarten"));
                cu_EnginePowerEntryKindergarten.Clear();
                cu_EnginePowerEntryKindergarten.SendKeys("444");
            }
            else
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                if (name != null)
                {
                    cu_NameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_NameEntryKindergarten.SendKeys("i add name for Kindergarten");
                }
                IWebElement cu_ShipTypeEntryKindergarten = driver.FindElement(By.Id("CU_ShipTypeEntryKindergarten"));
                cu_ShipTypeEntryKindergarten.Clear();
                cu_ShipTypeEntryKindergarten.SendKeys("10");
                IWebElement cu_CrewEntryKindergarten = driver.FindElement(By.Id("CU_CrewEntryKindergarten"));
                cu_CrewEntryKindergarten.Clear();
                cu_CrewEntryKindergarten.SendKeys("12345");
                IWebElement cu_EnginePowerEntryKindergarten = driver.FindElement(By.Id("CU_EnginePowerEntryKindergarten"));
                cu_EnginePowerEntryKindergarten.Clear();
                cu_EnginePowerEntryKindergarten.SendKeys("666667");
            }
        }

        private string InsertKindergartenData_ReturnName(IWebDriver driver, bool isOneOrTwo = false)
        {
            Guid returnGuid = Guid.NewGuid();
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                cu_NameEntryKindergarten.SendKeys(returnGuid.ToString());
                IWebElement cu_ShipTypeEntryKindergarten = driver.FindElement(By.Id("CU_ShipTypeEntryKindergarten"));
                cu_ShipTypeEntryKindergarten.Clear();
                cu_ShipTypeEntryKindergarten.SendKeys("40");
                IWebElement cu_CrewEntryKindergarten = driver.FindElement(By.Id("CU_CrewEntryKindergarten"));
                cu_CrewEntryKindergarten.Clear();
                cu_CrewEntryKindergarten.SendKeys("67890");
                IWebElement cu_EnginePowerEntryKindergarten = driver.FindElement(By.Id("CU_EnginePowerEntryKindergarten"));
                cu_EnginePowerEntryKindergarten.Clear();
                cu_EnginePowerEntryKindergarten.SendKeys("444");
            }
            else
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                cu_NameEntryKindergarten.SendKeys(returnGuid.ToString());
                IWebElement cu_ShipTypeEntryKindergarten = driver.FindElement(By.Id("CU_ShipTypeEntryKindergarten"));
                cu_ShipTypeEntryKindergarten.Clear();
                cu_ShipTypeEntryKindergarten.SendKeys("10");
                IWebElement cu_CrewEntryKindergarten = driver.FindElement(By.Id("CU_CrewEntryKindergarten"));
                cu_CrewEntryKindergarten.Clear();
                cu_CrewEntryKindergarten.SendKeys("12345");
                IWebElement cu_EnginePowerEntryKindergarten = driver.FindElement(By.Id("CU_EnginePowerEntryKindergarten"));
                cu_EnginePowerEntryKindergarten.Clear();
                cu_EnginePowerEntryKindergarten.SendKeys("666667");
            }
            return returnGuid.ToString();
        }
    }
}
