using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QnABot.Security;
using QnABot.Model;
using Newtonsoft.Json;
using System.Net;

namespace QnABot.API
{
    public class QnaApi
    {
        public static QnAMakerResult GetFirstQnaAnswer(string inputText)
        {
            var query = inputText;

            // QnA connection
            string responseString = string.Empty;
            string returnResult = string.Empty;

            var builder = new UriBuilder($"{Credentials.QNA_MAKER_URI_BASE}/knowledgebases/{Credentials.QNA_KNOWLEDGE_BASE_ID}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", Credentials.QNA_SUBSCRIPTION_KEY);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
            }

            //De-serialize the response
            QnAMakerResult qnaResponse = null;

            try
            {
                qnaResponse = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
            }
            catch (Exception e)
            {
                // Implement exception handling
            }

            return qnaResponse;
        }
    }
}