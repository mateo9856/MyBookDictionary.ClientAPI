﻿using MyBookDictionary.Model.Enums;
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
        private readonly object responseLock = new object();
        public HttpClient client { get; set; }

        public ResultWebsitesFinder(string keyPhrase)
        {
            KeyPhrase = ConvertPhrase(keyPhrase);
            client = new HttpClient();
        }

        private object GenerateAlgorithm(HttpResponseMessage? response)
        {
            if(response == null) return null;

            var reader = new StreamReader(response.Content.ReadAsStream(), Encoding.UTF8).ReadToEnd();//value must be synchronizely

            //TODO: HTML converter algorithm

            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<object> Find(string phrase)
        {
            var searchUri = new Uri(string.Format("https://www.google.com/search?q={0}", KeyPhrase));
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36");
            try
            {
                HttpResponseMessage? response = null;

                response = await client.GetAsync(searchUri);

                if (response.IsSuccessStatusCode)
                {
                    return GenerateAlgorithm(response);

                } else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    Thread.Sleep(5000);
                    response = await client.GetAsync(searchUri);
                    if (response.IsSuccessStatusCode)
                    {
                        return GenerateAlgorithm(response);
                    }
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