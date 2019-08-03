using Insta.Graph.API.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Insta.Graph.API.Logic
{
    public class InstagramManager
    {
        #region private vars

        private string baseUrl = "https://graph.facebook.com/v3.2/17841408385455309?fields=";
        private string access_token = "";
        private string _token = string.Empty;
        private string _impressionInsightDescription = "impressions";

        #endregion

        public InstagramManager(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _token = this.access_token;
            }
            else
            {
                _token = token;
            }
        }

        private string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private List<DTO.InstagramResult> DoMediaSearch()
        {
            // get the list of media items
            // parse out the reponse and the fields we want
            // convert to DTOs and return

            string mediaFields = "media%7Bmedia_url%2Cmedia_type%2Ccomments_count%2Clike_count%2Ctimestamp%2Cpermalink%2Ccaption%7D";
            string mediaSearchUrl = this.baseUrl + mediaFields + "&access_token=" + _token;

            List<InstagramResult> list = new List<InstagramResult>();

            //invoke the request
            string jsonResult = this.Get(mediaSearchUrl);

            // convert to json annotated object
            InstagramResult instagramResult = new InstagramResult();
            instagramResult = JsonConvert.DeserializeObject<InstagramResult>(jsonResult);

            if (instagramResult != null && instagramResult.media != null)
            {
                foreach (MediaData mediaData in instagramResult.media.data)
                {
                    list.Add(instagramResult);
                }
            }

            return list;
        }

        public List<Entity.Media> GetMedia()
        {
            // invoke the private method - DoMediaSearch()
            List<InstagramResult> instagramResults = this.DoMediaSearch();
            List<Entity.Media> mediaModels = new List<Entity.Media>();

            //map from the JSON/DTO returned by DoMediaSearch() to the Domain Entities
            foreach (InstagramResult instagramResult in instagramResults)
            {
                foreach (MediaData mediaData in instagramResult.media.data)
                {
                    mediaModels.Add(
                        new Entity.Media
                        {
                            id = mediaData.id,
                            like_count = mediaData.like_count,
                            comments_count = mediaData.comments_count,
                            impression_count = GetMediaImpressionValue(GetMediaImpressionsInsight(mediaData)),
                            media_url = mediaData.media_url,
                            permalink = mediaData.permalink,
                            Comments = GetMediaCommentsEntities(mediaData),
                            timestamp = mediaData.timestamp,
                            DateCreated = mediaData.DateCreated
                        });
                }
            }
            return mediaModels;
        }

        private DTO.InstagramInsight GetMediaImpressionsInsight(MediaData mediaData)
        {
            string impressionsUrl = "https://graph.facebook.com/v3.2/" + mediaData.id + "/insights/?metric=impressions&access_token=" + _token;

            InstagramInsight instagramInsight = new InstagramInsight();

            string jsonResult = this.Get(impressionsUrl);

            instagramInsight = JsonConvert.DeserializeObject<InstagramInsight>(jsonResult);

            return instagramInsight;
        }

        private int GetMediaImpressionValue(InstagramInsight insight)
        {
            return insight.data.Find(i => i.name == _impressionInsightDescription).values[0].value;
        }

        private List<Entity.Comment> GetMediaCommentsEntities(MediaData mediaData)
        {
            Comments commentsDTOs = GetMediaCommentsDTO(mediaData);
            List<Entity.Comment> comments = new List<Entity.Comment>();

            foreach (DTO.CommentData commentData in commentsDTOs.data)
            {
                comments.Add(new Entity.Comment { id = commentData.id, text = commentData.text });
            }

            return comments;
        }

        private DTO.Comments GetMediaCommentsDTO(MediaData mediaData)
        {
            string commentsUrl = "https://graph.facebook.com/v3.2/" + mediaData.id + "/comments?access_token=" + _token;

            Comments comments = new Comments();

            string jsonResult = this.Get(commentsUrl);

            comments = JsonConvert.DeserializeObject<Comments>(jsonResult);

            return comments;
        }

    }
}
