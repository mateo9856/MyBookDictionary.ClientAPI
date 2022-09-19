using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.NoteFinder
{
    public class ResultWebsitesFinder : IDisposable
    {
        public string KeyPhrase { get; set; }

        public HttpClient client { get; set; }

        public ResultWebsitesFinder(string keyPhrase)
        {
            KeyPhrase = ConvertPhrase(keyPhrase);
            client = new HttpClient();
        }

        public async Task<object> Find(string phrase)
        {
            var searchUri = new Uri(string.Format("https://www.google.com/search?q={0}", KeyPhrase));

            try
            {
                var response = await client.GetAsync(searchUri);
                if (response.IsSuccessStatusCode)
                    return response.Content; //analyze this

                return response.StatusCode; //analyze status codes

            } catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string ConvertPhrase(string phrase) => phrase.Replace(' ', '+');

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
