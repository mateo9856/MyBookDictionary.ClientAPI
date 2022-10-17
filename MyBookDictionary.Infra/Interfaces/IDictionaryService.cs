using MyBookDictionary.Application.Requests.Dictionary;
using MyBookDictionary.Infra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Interfaces
{
    public interface IDictionaryService
    {
        Task<object> GenerateByKeywordAsync(string phrase);
        Task<object> GenerateByTagsAsync(FindByTags tags);
        Task<IEnumerable<string>> GenerateNote(string address);
    }
}
