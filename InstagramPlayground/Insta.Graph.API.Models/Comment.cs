using System;
using System.Collections.Generic;
using System.Text;

namespace Insta.Graph.Entity
{
    public class Comment
    {
        public string id { get; set; }
        public string text { get; set; }
        public TextAnalyticsInsight TextAnalyticsInsight { get; set; }

        public Comment()
        {
            TextAnalyticsInsight = new TextAnalyticsInsight();
        }
    }
}
