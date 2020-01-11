using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insta.Graph.Entity.CognitiveServices
{
    public class EntityRecord
    {
        public string Name { get; set; }
        public string WikipediaLanguage { get; set; }
        public string WikipediaId { get; set; }
        public string WikipediaUrl { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
    }
}
