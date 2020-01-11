using Insta.Graph.Entity;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Insta.Graph.API.Logic
{
    public class TextAnalyticsManager
    {
        #region keys
        private static string _azureRegion = "YOU AZURE ENDPOINT";
        private static string _textAnalyticsKey = "YOUR AZURE KEY";
        #endregion

        #region credentials class
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            private readonly string apiKey;

            public ApiKeyServiceClientCredentials(string apiKey)
            {
                this.apiKey = apiKey;
            }

            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException("request");
                }
                request.Headers.Add("Ocp-Apim-Subscription-Key", this.apiKey);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }
        #endregion

        public static TextAnalyticsClient AuthenticateTextAnalytics(string endpoint, string key)
        {
            TextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint, };

            return client;
        }

        public KeyPhraseResult ProcessKeyPhrases(string documentid, string text)
        {
            TextAnalyticsClient client = AuthenticateTextAnalytics(_azureRegion, _textAnalyticsKey);

            KeyPhraseResult result = client.KeyPhrases(text);

            return result;
        }

        public SentimentResult ProcessSentiment(string documentid, string text)
        {
            TextAnalyticsClient client = AuthenticateTextAnalytics(_azureRegion, _textAnalyticsKey);

            SentimentResult results = client.Sentiment(text, "en");

            return results;
        }

        public EntitiesResult ProcessEntities(string documentid, string text)
        {
            TextAnalyticsClient client = AuthenticateTextAnalytics(_azureRegion, _textAnalyticsKey);

            EntitiesResult result = client.Entities(text);

            return result;
        }

        public TextAnalyticsInsight GetInsights(string documentid, string text)
        {
            // create the TextAnalyticsInsight object 
            if (!string.IsNullOrEmpty(text))
            {
                TextAnalyticsInsight textAnalyticsInsight = new TextAnalyticsInsight();
                EntitiesResult entitiesResult = this.ProcessEntities(documentid, text);
                KeyPhraseResult keyPhraseResult = this.ProcessKeyPhrases(documentid, text);
                SentimentResult sentimentResult = this.ProcessSentiment(documentid, text);

                foreach (EntityRecord record in entitiesResult.Entities)
                {
                    textAnalyticsInsight.EntityRecords.Add(new Entity.CognitiveServices.EntityRecord
                    {
                        Name = record.Name,
                        SubType = record.SubType,
                        Type = record.Type,
                        WikipediaId = record.WikipediaId,
                        WikipediaLanguage = record.WikipediaLanguage,
                        WikipediaUrl = record.WikipediaUrl
                    });
                }

                foreach (string keyPhrase in keyPhraseResult.KeyPhrases)
                {
                    textAnalyticsInsight.KeyPhrases.Add(keyPhrase);
                }

                textAnalyticsInsight.SentimentScore = sentimentResult.Score.Value;

                // map the CogServices models to our custom model TextAnalyticsInsight which
                // contains all TextAnalytics insights for the paramter Text
                return textAnalyticsInsight;
            }

            return new TextAnalyticsInsight();
        }

    }
}
