using MyBookDictionary.Application.Requests.Dictionary;
using MyBookDictionary.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Services
{
    public class DictionaryService : IDictionaryService
    {
        public Task<object> GenerateByKeywordAsync(string phrase)
        {
            throw new NotImplementedException();
        }

        public Task<object> GenerateByTagsAsync(FindByTags tags)
        {
            throw new NotImplementedException();
        }
    }
}
