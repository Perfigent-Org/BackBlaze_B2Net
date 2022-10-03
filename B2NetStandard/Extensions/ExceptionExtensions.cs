using System;
using System.Collections.Generic;
using System.Linq;

namespace B2NetStandard.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string ToAllString(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException).Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }
    }
}
