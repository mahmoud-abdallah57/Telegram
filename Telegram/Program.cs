using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Telegram
{
    class telegramMessage
    {

        public string numberOfView { get; set; }
        public string commenterName { get; set; }
        public string title { get; set; }

        public string image { get; set; }

        public string details { get; set; }


    }
    class Program
    {

        private const string URL = "http://t.me/s/Mostafasharaawy";
        private static string urlParameters = "?after=1";

        static void Main(string[] args)
        {
            List<telegramMessage> messages = new List<telegramMessage>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
          
            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                Console.WriteLine("{0}", dataObjects);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(dataObjects);

                string matchResultDivId = "tgme_widget_message_bubble";
                string xpath = String.Format("//div[@class='{0}']", matchResultDivId);
                var people = doc.DocumentNode.SelectNodes(xpath);

                foreach (var person in people)
                {
                    string details = person.SelectSingleNode("//div[@class='link_preview_description']//text()").InnerText.Trim();

                    string numberOfView = person.SelectSingleNode("//span[@class='tgme_widget_message_views']//text()").InnerText.Trim();
                    telegramMessage newMessage = new telegramMessage()
                    {
                        details = details,
                        numberOfView = numberOfView


                    };

                    messages.Add(newMessage);
                }

            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            //Make any other calls using HttpClient here.

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }
    }
}
