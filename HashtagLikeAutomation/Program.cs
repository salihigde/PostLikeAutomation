using HashtagLikeAutomation.Services;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HashtagLikeAutomation
{
    //This console application likes recent posts of the specified hashtags
    class Program
    {
        const string INSTAGRAM_USER_NAME = "test_username";
        const string INSTAGRAM_PASSWORD = "test_password";

        static void Main(string[] args)
        {
            ThreadStart likeAutomationMethod = () => ExecuteLikeAutomation();
            Thread likeAutomationThread = new Thread(likeAutomationMethod);
            likeAutomationThread.Start();
        }

        private static void ExecuteLikeAutomation()
        {
            InstagramAutomationService.Current.Init();

            InstagramAutomationService.Current.Login(INSTAGRAM_USER_NAME, INSTAGRAM_PASSWORD);

            Thread.Sleep(3000);

            if (InstagramAutomationService.Current.IsLoginSuccess())
            {
                while (true)
                {
                    List<string> hashtags = DataService.Current.GetAllHashtags();

                    foreach (var hashtag in hashtags)
                    {
                        List<string> postUrls = InstagramAutomationService.Current.GetRecentPosts(hashtag);

                        foreach (var postUrl in postUrls)
                        {
                            InstagramAutomationService.Current.LikePost(postUrl);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("ERROR: User is not logged-in");
            }
        }
    }
}
