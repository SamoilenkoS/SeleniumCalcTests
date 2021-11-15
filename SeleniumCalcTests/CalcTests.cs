using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace SeleniumCalcTests
{
    public class Tests
    {
        private IWebDriver _driver;
        private string _path = "file:///C:/Users/Sviatoslav_Samoilenk/Desktop/CalculatorWeb/index.html";
        private IWebElement _numA;
        private IWebElement _numB;
        private IWebElement _result;
        private IWebElement _calculate;
        private SelectElement _operation;

        [OneTimeSetUp]
        public void Initialize()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl(_path);
            _numA = _driver.FindElement(By.Id("numA"));
            _numB = _driver.FindElement(By.Id("numB"));
            _operation = new SelectElement(
                _driver.FindElement(By.Id("operation")));
            _result = _driver.FindElement(By.Id("result"));
            _calculate = _driver.FindElement(By.ClassName("calculate"));
        }

        [OneTimeTearDown]
        public void Deconstruct()
        {
            _driver.Quit();
        }

        [TestCaseSource(nameof(GetValues))]
        public void Calc_WhenCalled_ShouldOperateTwoNumbers(OperationData operationData)
        {
            _numA.Clear();
            _numA.SendKeys(operationData.X.ToString());
            _numB.Clear();
            _numB.SendKeys(operationData.Y.ToString());
            _operation.SelectByText(operationData.Sign);

            _calculate.Click();

            Assert.AreEqual(GetResult(operationData),
                _result.GetAttribute("value"));
        }

        static string GetResult(OperationData operationData)
        {
            string result = string.Empty;
            switch (operationData.Sign)
            {
                case "+":
                    result = (operationData.X + operationData.Y).ToString();
                    break;
                case "-":
                    result = (operationData.X - operationData.Y).ToString();
                    break;
                case "*":
                    result = (operationData.X * operationData.Y).ToString();
                    break;
                case "/":
                    if(operationData.Y != 0)
                    {
                        result = (operationData.X / operationData.Y).ToString();
                    }
                    else
                    {
                        result = "Could not divide by zero!";
                    }
                    break;
            }

            return result;
        }

        static IEnumerable<OperationData> GetValues()
        {
            var signs = new[] { "+", "-", "*", "/" };
            foreach (var sign in signs)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        yield return new OperationData
                        {
                            X = i,
                            Y = j,
                            Sign = sign
                        };
                    }
                }
            }
            
        }
    }
}