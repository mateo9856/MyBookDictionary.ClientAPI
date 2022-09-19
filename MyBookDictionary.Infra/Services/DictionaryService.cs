using MyBookDictionary.Application.Requests.Dictionary;
using MyBookDictionary.Infra.Interfaces;
using MyBookDictionary.Infra.NoteFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Services
{
    public class DictionaryService : IDictionaryService
    {
        private ResultWebsitesFinder _websitesFinder;

        public async Task<object> GenerateByKeywordAsync(string phrase)
        {
            _websitesFinder = new ResultWebsitesFinder(phrase);
            return await _websitesFinder.Find(phrase);
        }

        public Task<object> GenerateByTagsAsync(FindByTags tags)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _websitesFinder.Dispose();
        }

    }
}
