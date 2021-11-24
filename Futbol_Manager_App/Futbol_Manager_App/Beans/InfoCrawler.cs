using System;

namespace Futbol_Manager_App.Beans
{
    [Serializable]
    public class InfoCrawler
    {
        public InfoCrawler(string title)
        {
            Title = title;
        }
        public string Title { get; set; }
    }
}
