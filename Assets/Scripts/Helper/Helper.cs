using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    public static class Helper
    {
        public static TValue ComputeIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> compute)
        {
            if (!dictionary.TryGetValue(key, out var value)) value = compute(key);
            dictionary.TryAdd(key, value);
            return value;
        }

        private static bool InCircle(Vector2 point, Vector2 circlePoint, float radius)
        {
            return (point - circlePoint).sqrMagnitude <= radius * radius;
        }

        public static IEnumerable<Vector2> PointsInCircle(this Vector2 circlePos, float radius, float scale)
        {
            var minX = circlePos.x - radius;
            var maxX = circlePos.x + radius;

            var minY = circlePos.y - radius;
            var maxY = circlePos.y + radius;

            for (var y = minY; y <= maxY; y += scale)
            for (var x = minX; x <= maxX; x += scale)
                if (InCircle(new Vector2(x, y), circlePos, radius))
                    yield return new Vector2(x, y);
        }
    }
}