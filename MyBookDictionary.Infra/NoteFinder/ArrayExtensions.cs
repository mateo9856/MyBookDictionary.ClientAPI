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
        public static int FindIndexHTMLClosureIndex(this IEnumerable<string> val, string tag) 
        {
            var valToList = val.ToList();
            List<int> ClosureIndexes = new List<int>();
            List<int> CutIndexes = new List<int>();
            bool ClosureChanged = false;
            int closureToFind = 0;
            int index = 0;

            foreach(var item in val)
            {
                if (item.Contains($"<{tag}")) 
                {
                    closureToFind++;
                    ClosureIndexes.Add(index);
                }
                if (item.Contains($"</{tag}") && ClosureIndexes.Count() > 0) {
                    ClosureChanged = true;
                    var lastClosure = ClosureIndexes.Last();
                    int removeRange = index - lastClosure;
                    valToList.RemoveRange(lastClosure, removeRange);
                    CutIndexes.Add(index);
                    index = lastClosure;
                    ClosureIndexes.Remove(lastClosure);
                }
                if (ClosureChanged && ClosureIndexes.Count() == 0) 
                {
                    return CutIndexes.Last();
                }
                
                index++;
            }
            return index;
        }
    }
}
