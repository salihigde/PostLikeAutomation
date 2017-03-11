using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashtagLikeAutomation.Services
{
    public class DataService
    {
        private static DataService _service;

        public static DataService Current
        {
            get
            {
                if (_service == null)
                    _service = new DataService();

                return _service;
            }
        }

        public List<string> GetAllHashtags()
        {
            List<string> hashtags = new List<string>();

            hashtags.Add("travel");
            hashtags.Add("travelling");
            hashtags.Add("food");

            return hashtags;
        }
    }
}
