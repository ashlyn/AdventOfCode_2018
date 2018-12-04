using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
  public static T Mode<T>(this IEnumerable<T> enumerable)
  {
    return enumerable.GroupBy(n => n)
      .OrderByDescending(g => g.Count())
      .Select(g => g.Key)
      .FirstOrDefault();
  }

  public static ValueTuple<T, int> ModeWithCount<T>(this IEnumerable<T> enumerable)
  {
    return enumerable.GroupBy(n => n)
      .OrderByDescending(g => g.Count())
      .Select(g => ValueTuple.Create(g.Key, g.Count()))
      .FirstOrDefault();
  }
}