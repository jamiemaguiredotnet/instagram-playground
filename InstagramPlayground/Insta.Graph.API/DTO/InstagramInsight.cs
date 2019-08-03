using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insta.Graph.API.DTO
{
    //json classes that represent the messages we receive from the Graph API
    public class Value
    {
        [JsonProperty("value")]
        public int value { get; set; }

        [JsonProperty("end_time")]
        public string end_time { get; set; }
    }

    public class Insight
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("period")]
        public string period { get; set; }

        [JsonProperty("values")]
        public IList<Value> values { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }
    }

    public class InstagramInsight
    {
        [JsonProperty("data")]
        public List<Insight> data { get; set; }

        public InstagramInsight()
        {
            this.data = new List<Insight>();
        }
    }


}
