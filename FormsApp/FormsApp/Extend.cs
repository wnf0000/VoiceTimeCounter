using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace FormsApp
{
    public static class Extend
    {
        public static void For<T>(this IEnumerable<T> o, Action<int> action)
        {
            if (o == null || action == null) return;
            for (int i = 0; i < o.Count(); i++)
            {
                action(i);
            }

        }
        public static void For<T>(this IEnumerable<T> o, int startIndex, int endIndex, Action<int> action)
        {
            if (o == null || action == null) return;
            for (int i = startIndex; i <= endIndex; i++)
            {
                action(i);
            }

        }
        public static void ForEach<T>(this IEnumerable<T> o, Action<T> action)
        {
            if (o == null || action == null) return;
            foreach (var O in o)
            {
                action(O);
            }
        }
        #region Serialize
        public static string Serialize(this object o)
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string result = JsonConvert.SerializeObject(o, Formatting.None, settings);
            return result;
        }
        public static object Deserialize(this string o)
        {
            if (string.IsNullOrEmpty(o)) return null;
            return JsonConvert.DeserializeObject(o);
        }
        public static T Deserialize<T>(this string o) where T : class
        {
            if (string.IsNullOrEmpty(o)) return null;
            return JsonConvert.DeserializeObject<T>(o);
        }
        #endregion
        #region ObservableCollection
        public static void Sort<T>(this ObservableCollection<T> Collection)
        {
            Collection.Sort(Comparer<T>.Default);
        }

        public static void Sort<T>(this ObservableCollection<T> Collection, IComparer<T> comparer)
        {
            if(Collection==null) return;
            
            int i, j;
            T index;
            for (i = 1; i < Collection.Count; i++)
            {
                index = Collection[i];
                j = i;
                while ((j > 0) && (comparer.Compare(Collection[j - 1], index) >0))
                {
                    Collection[j] = Collection[j - 1];
                    j = j - 1;
                }
                Collection[j] = index;
            }
        }
        #endregion
    }
}
