using System.Collections.Generic;
using System.Linq;

namespace DOT.Models
{
    static public class Helper
    {
        public static bool SharesAnyValueWith<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            return a.Intersect(b).Any();
        }
    }
}
