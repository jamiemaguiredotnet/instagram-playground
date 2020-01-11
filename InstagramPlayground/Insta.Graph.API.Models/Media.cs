using System;
using System.Collections.Generic;
using System.Text;

namespace Insta.Graph.Entity
{
    /// <summary>
    /// Representa a Media object from the Graph API.  
    /// Messages/DTOs from the Graph API get converted to Entities as we don't
    /// need every property from the deserialized Graph API messages.
    /// </summary>
    public class Media
    {
        public string id { get; set; }
        public int MediaDataId { get; set; }
        public string caption { get; set; }
        public TextAnalyticsInsight CaptionInsights { get; set; }
        public string media_url { get; set; }
        public int comments_count { get; set; }
        public int like_count { get; set; }
        public int impression_count { get; set; }
        public string permalink { get; set; }
        public ComputerVisionInsight VisionInsights { get; set; }
        public List<Comment> Comments { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime DateCreated { get; set; }

        public Media()
        {
            this.Comments = new List<Comment>();
            this.CaptionInsights = new TextAnalyticsInsight();
            this.VisionInsights = new ComputerVisionInsight();
        }
    }
}
