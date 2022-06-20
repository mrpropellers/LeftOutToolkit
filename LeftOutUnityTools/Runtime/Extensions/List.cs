using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace LeftOut.Extensions
{
    public static class ListExtensions
    {
        public static T SelectRandom<T>(this List<T> self) =>
            self.Count > 0
            ? self[Random.Range(0, self.Count)]
            : throw new ArgumentOutOfRangeException(
                $"Can't {nameof(SelectRandom)} from an empty list.");
    }
}
