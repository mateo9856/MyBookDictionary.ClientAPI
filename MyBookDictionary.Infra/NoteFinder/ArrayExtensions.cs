using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.NoteFinder
{
    public static class ArrayExtensions
    {
        public static IEnumerable<int> GetAllIndexOf(this IEnumerable<string> val, string text)
        {
            var EnumerableToArr = val.ToArray();

            for(int i = 0; i < EnumerableToArr.Length; i++)
            {
                if(EnumerableToArr[i].Contains(text))
                {
                    yield return i;
                }
            }
        }
    }
}
