using MyBookDictionary.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
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
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36");
            try
            {
                var response = await client.GetAsync(searchUri);
                if (response.IsSuccessStatusCode)
                {
                    var reader = await new StreamReader(await response.Content.ReadAsStreamAsync(), Encoding.UTF8).ReadToEndAsync();//value must be synchronizely

                    //TODO: HTML converter algorithm

                    return await response.Content.ReadAsStringAsync();
                }


                var responseCode = (int)response.StatusCode;

                try
                {
                    var CodeDesc = (StatusDescriptions)responseCode;
                    return CodeDesc.GetEnumDescription();

                } catch (Exception ex)
                {
                    throw new Exception("Unexpected error!");
                }

                return null; //analyze status codes

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
