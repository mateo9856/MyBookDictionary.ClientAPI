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
            try
            {
                _websitesFinder = new ResultWebsitesFinder(phrase);
                return await _websitesFinder.Find(phrase, Helpers.SearchType.GoogleSearch);
            } catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task<object> GenerateByTagsAsync(FindByTags tags)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _websitesFinder.Dispose();
        }

        public async Task<IEnumerable<string>> GenerateNote(string address)
        {
            try
            {
                _websitesFinder = new ResultWebsitesFinder(address);
//TODO:Implement
                var finder = await _websitesFinder.Find(address, Helpers.SearchType.GenerateNote);
                return new List<string>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
