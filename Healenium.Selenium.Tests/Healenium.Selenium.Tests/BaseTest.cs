using System;
using Autofac;
using Healenium.Selenium.pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Healenium.Selenium.Tests
{
    public class BaseTest
    {
        static protected IWebDriver _driver;
        public static ITestEnvPage _testEnvPage;
        public static ICallbackPage _callbackPage;
        public static IMainPage _mainPage;

        [SetUp]
        [Obsolete]
        public static void SetUp()
        {
            var browserOptions = new ChromeOptions();
            browserOptions.PlatformName = "Windows 10";
            browserOptions.BrowserVersion = "latest"
            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("username", "oauth-manisharajamani97-3541d");
            sauceOptions.Add("accessKey", "e4fb9dca-9c93-4566-80ea-c254d12614e9");
            sauceOptions.Add("build", "<your build id>");
            sauceOptions.Add("name", "<your test name>");
            browserOptions.AddAdditonalOption("sauce:options", sauceOptions);

 

            var uri = new Uri("https://ondemand.eu-central-1.saucelabs.com:443/wd/hub");
            var driver = new RemoteWebDriver(uri, browserOptions);
            
            //var optionsChrome = new ChromeOptions();
            //optionsChrome.AddArguments("--no-sandbox");
            //_driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), optionsChrome);

            //var options = new FirefoxOptions();
            //_driver = new RemoteWebDriver(new Uri("http://localhost:8085"), options);

            var builder = new ContainerBuilder();
            builder.Register(c => new TestEnvPage(_driver)).As<ITestEnvPage>();
            builder.Register(c => new CallbackTestPage(_driver)).As<ICallbackPage>();
            builder.Register(c => new MainPage(_driver)).As<IMainPage>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                _testEnvPage = scope.Resolve<ITestEnvPage>();
                _callbackPage = scope.Resolve<ICallbackPage>();
                _mainPage = scope.Resolve<IMainPage>();
            }

        }

        [TearDown]
        public static void AfterAll()
        {
            _driver.Quit();
        }
    }
}

