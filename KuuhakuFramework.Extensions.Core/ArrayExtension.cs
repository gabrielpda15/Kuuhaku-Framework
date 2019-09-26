using System;
using System.Collections.Generic;
using System.Linq;

namespace KuuhakuFramework.Extensions
{
    public static class ArrayExtension
    {
        public static bool Compare<T>(this IEnumerable<T> array, IEnumerable<T> obj)
        {
            if (array.Count() == obj.Count())
            {
                for (int i = 0; i < array.Count(); i++)
                {
                    if (!array.ElementAt(i).Equals(obj.ElementAt(i)))
                        return false;
                }
                return true;
            }
            return false;
        }

        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (var item in array)
            {
                action(item);
            }
        }

        public static TOut[] ForEach<T, TOut>(this IEnumerable<T> array, Func<T, TOut> func, IList<TOut> initial)
        {
            return array.ForEach(func, (a, b) => { a.Add(b); return a; }, k => k.ToArray(), initial);
        }

        public static TResult ForEach<T, TOut, TStep, TResult>(this IEnumerable<T> array, Func<T, TOut> func, Func<TStep, TOut, TStep> aggregator, Func<TStep, TResult> resultor, TStep initialValue = default)
        {
            var results = array.ForEach(func);

            var step = initialValue;

            foreach (var item in results)
            {
                step = aggregator(step, item);
            }

            return resultor(step);
        }

        public static IEnumerable<TOut> ForEach<T, TOut>(this IEnumerable<T> array, Func<T, TOut> func)
        {
            var results = new List<TOut>();

            foreach (var item in array)
            {
                results.Add(func(item));
            }

            return results;
        }

        public static IEnumerable<TOut> ForEach<T, TOut>(this IEnumerable<T> array, Func<T, IEnumerable<TOut>> func)
        {
            var results = new List<TOut>();

            foreach (var item in array)
            {
                results.AddRange(func(item));
            }

            return results;
        }
    }
}
