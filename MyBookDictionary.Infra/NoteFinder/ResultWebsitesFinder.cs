﻿using MyBookDictionary.Application.WebSearch;
using MyBookDictionary.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
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

            var result = response.Content.ReadAsStringAsync().Result;

            //find first index of body

            var cutHtmlHeadTags = result.Substring(result.IndexOf("<body"));
            var cutClosureBody = cutHtmlHeadTags.Substring(0, cutHtmlHeadTags.LastIndexOf("</body") + 7);

            var addNewLinesToArr = Regex.Replace(cutClosureBody, @">", ">\n").Split('\n');
            var searchIndex = addNewLinesToArr.First(d => d.Contains("div") && d.Contains("class") && d.Contains(string.Concat('\u0022', "yuRUbf", '\u0022')));
            var takeArr = addNewLinesToArr.Skip(Array.IndexOf(addNewLinesToArr ,searchIndex)).ToList();
            var DifferentDivs = takeArr.Count(c => c.Contains("</div")) - takeArr.Count(c => c.Contains("<div"));

            for(int i = 0; i < DifferentDivs; i++)
            {
                var lastIndex = takeArr.FindLastIndex(c => c.Contains("</div"));
                takeArr.RemoveAt(lastIndex);
            }

            var cutToLastDiv = takeArr.Take(takeArr.FindLastIndex(c => c.Contains("/div")) + 1).ToList();
            var indexesFromDiv = cutToLastDiv.GetAllIndexOf("yuRUbf").ToList();

            var divArrs = new List<string[]>();

            int divIndex = 0;
            foreach(var item in indexesFromDiv.Skip(1))
            {
                divArrs.Add(cutToLastDiv.Take(item - divIndex).ToArray());
                divIndex = item;
            }
      
            //TODO: Select items which contain link and description

            return string.Join(string.Empty, cutToLastDiv);
        }

        public async Task<FindByKeywordResponse> Find(string phrase)
        {
            var searchUri = new Uri(string.Format("https://www.google.com/search?q={0}", KeyPhrase));
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36");
            try
            {

                Thread.Sleep(3000);

                var response = await client.GetAsync(searchUri);

                if (response.IsSuccessStatusCode)
                {
                    var algo = GenerateAlgorithm(response);

                    return new FindByKeywordResponse { Status = "Success", Message = algo } ;

                }
                var responseCode = (int)response.StatusCode;

                try
                {
                    var CodeDesc = (StatusDescriptions)responseCode;

                    return new FindByKeywordResponse { Status = "Failed", Message = CodeDesc.GetEnumDescription() };

                } catch (Exception ex)
                {
                    throw new Exception("Unexpected error!");
                }

                return null;

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
