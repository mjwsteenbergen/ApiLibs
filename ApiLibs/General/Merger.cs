using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibs.General
{
    public static class Merger
    {
        public static List<T> Merge<T>(List<T> currentArray, List<T> secondList)
        {
            if (currentArray == null)
            {
                return secondList;
            }

            var currentList = currentArray.ToList();
            foreach (T item in secondList)
            {
                var index = currentList.IndexOf(item);
                if (index != -1)
                {
                    currentList.RemoveAt(index);
                }
                currentList.Add(item);
            }
            return currentList;
        }
    }
}
