using Insta.Graph.Entity.CognitiveServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insta.Graph.Entity
{
    public class TextAnalyticsInsight
    {
        public List<string> KeyPhrases { get; set; }
        public double SentimentScore { get; set; }
        public List<EntityRecord> EntityRecords { get; set; }

        public TextAnalyticsInsight()
        {
            this.EntityRecords = new List<EntityRecord>();
            this.KeyPhrases = new List<string>();
        }
    }
}
