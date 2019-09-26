using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.Extensions
{
    public static class ObjectExtension
    {
        public static TOutput Map<TInput, TOutput>(this TInput input, TOutput instance = null)
            where TInput : class
            where TOutput : class
        {
            var result = instance ?? Activator.CreateInstance<TOutput>();

            var propInput = input.GetType().GetProperties();
            var propOutput = result.GetType().GetProperties();

            var map = new
            {
                PropCommonInput = propOutput.ForEach(i =>
                {
                    var match = propInput.Where(o => i.Compare(o));
                    if (match.Count() == 1)
                        return match.Single();
                    return null;
                }, (a, b) => { a.Add(b); return a; }, k => k.ToArray(), new List<PropertyInfo>()),
                PropCommonOutput = propInput.ForEach(i =>
                {
                    var match = propOutput.Where(o => i.Compare(o));
                    if (match.Count() == 1)
                        return match.Single();
                    return null;
                }, (a, b) => { a.Add(b); return a; }, k => k.ToArray(), new List<PropertyInfo>())
            };

            for (int i = 0; i < map.PropCommonInput.Length; i++)
            {
                map.PropCommonOutput[i].SetValue(result, map.PropCommonInput[i].GetValue(input));
            }

            return result;
        }

        private static bool Compare(this PropertyInfo prop, PropertyInfo value)
        {
            return prop.Name == value.Name && prop.PropertyType == value.PropertyType;
        }

        public static TOutput Map<TInput, TOutput>(this TInput input, Func<TInput, TOutput> mapper) 
            where TInput : class 
            where TOutput : class
        {
            return mapper(input);
        }

        /*public static IEnumerable<TOutput> Map<TInput, TOutput>(this IEnumerable<TInput> input, Func<TInput, TOutput> mapper)
            where TInput : class
            where TOutput : class
        {

        }*/
    }
}
