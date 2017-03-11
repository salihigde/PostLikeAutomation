using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HashtagLikeAutomation.Services
{
    public class InstagramAutomationService
    {
        private static InstagramAutomationService _service;
        PhantomJSDriver _driver;

        private const int PAGE_WAIT_SECONDS = 5;
        private const string HTML_POST_CLASS_NAME = "_8mlbc";
        private const string LIKE_BUTTON_CLASS_NAME = "_soakw";
        private const string LIKE_BUTTON_OPEN = "coreSpriteHeartOpen";
        private const string LIKE_BUTTON_FULL = "coreSpriteHeartFull";

        private InstagramAutomationService() { }

        public static InstagramAutomationService Current
        {
            get
            {
                if (_service == null)
                    _service = new InstagramAutomationService();

                return _service;
            }
        }

        public void Init()
        {
            _driver = new PhantomJSDriver();
            _driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(PAGE_WAIT_SECONDS);
        }

        public void Login(string userName, string password)
        {
            string loginUrl = "https://instagram.com/accounts/login/";

            NavigateToUrl(loginUrl);

            var pageRoot = _driver.FindElement(By.Id("react-root"));

            var userNameElement = pageRoot.FindElement(By.Name("username"));
            var passwordElement = pageRoot.FindElement(By.Name("password"));
            var buttonElement = pageRoot.FindElement(By.TagName("button"));

            userNameElement.SendKeys(userName);
            passwordElement.SendKeys(password);

            buttonElement.Click();
        }

        public bool IsLoginSuccess()
        {
            var htmlElement = _driver.FindElement(By.TagName("html"));

            var htmlClass = htmlElement.GetAttribute("class");

            if (htmlClass.Contains("not-logged-in"))
            {
                return false;
            }

            return true;
        }

        public List<string> GetRecentPosts(string hashtag)
        {
            string searchUrl = "https://www.instagram.com/explore/tags/" + hashtag;

            NavigateToUrl(searchUrl);

            if (_driver.PageSource.Contains("error-container"))
            {
                return new List<string>();
            }

            var pageRoot = _driver.FindElement(By.Id("react-root"));

            var allPostHtml = pageRoot.FindElements(By.ClassName(HTML_POST_CLASS_NAME));

            List<string> postUrls = new List<string>();
            foreach (var i in allPostHtml)
            {
                var url = i.GetAttribute("href");
                postUrls.Add(url);
            }

            return postUrls;
        }

        public bool LikePost(string postUrl)
        {
            NavigateToUrl(postUrl);

            if (_driver.PageSource.Contains("error-container"))
            {
                return false;
            }

            var pageRoot = _driver.FindElement(By.Id("react-root"));

            var likeButtonElement = pageRoot.FindElement(By.ClassName(LIKE_BUTTON_CLASS_NAME));


            if (!IsPostLiked(likeButtonElement))
            {
                likeButtonElement.Click();

                Thread.Sleep(2000);

                var likeButtonClassAttribute = likeButtonElement.GetAttribute("class");
                if (likeButtonClassAttribute.Contains(LIKE_BUTTON_FULL))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsPostLiked(IWebElement likeButtonElement)
        {
            var likeButtonClassAttribute = likeButtonElement.GetAttribute("class");

            return likeButtonClassAttribute.Contains(LIKE_BUTTON_FULL);
        }

        private void NavigateToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(PAGE_WAIT_SECONDS);
        }
    }
}
