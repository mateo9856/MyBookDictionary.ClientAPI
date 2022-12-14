using MyBookDictionary.Application.WebSearch;
using MyBookDictionary.Infra.Helpers;
using MyBookDictionary.Model.Entities;
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

        public string[] ResponseWebsite(HttpResponseMessage? response) 
        {
            if (response == null) return null;

            var reader = new StreamReader(response.Content.ReadAsStream(), Encoding.UTF8).ReadToEnd();

            var result = response.Content.ReadAsStringAsync().Result;

            var cutHtmlHeadTags = result.Substring(result.IndexOf("<body"));
            var cutClosureBody = cutHtmlHeadTags.Substring(0, cutHtmlHeadTags.LastIndexOf("</body") + 7);
            var addNewLinesToArr = Regex.Replace(cutClosureBody, @">", ">\n").Split('\n');
            return addNewLinesToArr;
        }

        private IEnumerable<LinkDscriptor> GenerateAlgorithm(HttpResponseMessage? response)
        {
            var result = ResponseWebsite(response);

            var searchIndex = result.First(d => d.Contains("div") && d.Contains("class") && d.Contains(string.Concat('\u0022', "yuRUbf", '\u0022')));
            var takeArr = result.Skip(Array.IndexOf(result ,searchIndex)).ToList();
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
                cutToLastDiv.RemoveRange(0, item - divIndex);
                divIndex = item;
            }

            var Results = divArrs.Select(c => new LinkDscriptor()
            {
                Link = Regex.Match(c.FirstOrDefault(d => d.StartsWith("<a href")), @"(https?:[^""]*)").Value,
                Descriptor = Regex.Replace(string.Join(string.Empty, c.Where(d => d.Contains("h3")).Take(2)), @"<[^>]*>", string.Empty)
            });
            return Results;
        }

        private IEnumerable<string> GenerateNotes(HttpResponseMessage? response, ContentClasses[] elements) 
        {
            List<IEnumerable<string>> Result = new List<IEnumerable<string>>();
            var result = ResponseWebsite(response);
            var contents = result.Where(d => elements.Any(c => d.Contains(c.ContentClassName)));

            //TODO: Return zeros
            foreach (var content in contents) 
            {
                //check if it's start tag or value contains in class
                var elementTag = elements.FirstOrDefault(d => content.Contains(d.ContentClassName));

                if(elementTag != null && elementTag.IsTag)
                {
                    var IndexOfs = result.GetAllIndexOf(result.FirstOrDefault(d => d.Contains($"<{elementTag.ContentClassName}")));

                    foreach(var index in IndexOfs)
                    {
                        var FindexIndex = result.Skip(index).FindIndexHTMLClosureIndex(elementTag.ContentClassName);
                        var resultArr = result.Skip(index).Take(FindexIndex + 1);
                        Result.Add(resultArr);
                    }
                
                } 
                else if(elementTag != null && !elementTag.IsTag)
                {
                    var IndexOfs = result.GetAllIndexOf(result.First(d => Regex.IsMatch(d, @"class=\u0022.*\u0022") && !d.Contains("<body")));

                    foreach(var index in IndexOfs) {
                        var GetTag = result[index].FindHtmlTag();
                        var FinderIndex = result.Skip(index).FindIndexHTMLClosureIndex(GetTag);
                        var resultArr = result.Skip(index).Take(FinderIndex + 1);
                        Result.Add(resultArr);
                    }

                }

                var IndexFromResult = Array.IndexOf(result, content);

            }

            //TODO: Exclude text from Results
            foreach(var item in Result)
            {

            }

            return null;
        }

        public async Task<object> Find(string phrase, SearchType type, ContentClasses[] elements = null)
        {
            Uri? searchUri = null;

            switch(type)
            {
                case SearchType.GoogleSearch:
                    searchUri = new Uri(string.Format("https://www.google.com/search?q={0}", KeyPhrase));
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36");
                    break;
                case SearchType.GenerateNote:
                    searchUri = new Uri(phrase);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36");
                    break;
                case SearchType.GoogleTags:
                    //TODO: Tags logic
                    return null;
                    break;
                default:
                    return null;
            }
            try
            {

                Thread.Sleep(3000);

                var response = await client.GetAsync(searchUri);

                if (response.IsSuccessStatusCode && type == SearchType.GoogleSearch)
                {
                    var algo = GenerateAlgorithm(response);

                    return new FindByKeywordResponse { Status = "Success", Message = algo } ;

                }
                else if(response.IsSuccessStatusCode && type == SearchType.GenerateNote)
                {
                    var algo = GenerateNotes(response, elements);
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
