using MyBookDictionary.Infra.Common;
using MyBookDictionary.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Interfaces
{
    public interface IContextClassService
    {
        Task<ContentClasses[]> GetFromParam(params string[] contents);
        Task<ContentClasses[]> AllParams();
    }
}
