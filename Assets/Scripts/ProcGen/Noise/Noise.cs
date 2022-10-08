using System;
using System.Linq;
using Assets.Scripts.World.Settings;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.ProcGen.Noise
{
    public static class Noise
    {
        public static NoiseMap GenerateNoiseMap(this NoiseSettings settings, int mapWidth, int mapHeight,
            Vector2 offset)
        {
            var noiseMap = new float[mapWidth, mapHeight];

            var prng = new Random(settings.Seed);
            var octaveOffsets = new Vector2[settings.Octaves];

            float maxPossibleHeight = 0;
            float amplitude = 1;

            for (var i = 0; i < settings.Octaves; i++)
            {
                var offsetX = prng.Next(-100000, 100000) + offset.x + settings.SampleCentre.x;
                var offsetY = prng.Next(-100000, 100000) - offset.y - settings.SampleCentre.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= settings.Persistence;
            }

            var maxLocalNoiseHeight = float.MinValue;
            var minLocalNoiseHeight = float.MaxValue;

            var halfWidth = mapWidth / 2f;
            var halfHeight = mapHeight / 2f;


            for (var x = 0; x < mapWidth; x++)
            for (var y = 0; y < mapHeight; y++)
            {
                amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (var i = 0; i < settings.Octaves; i++)
                {
                    var sampleX = (x - halfWidth + octaveOffsets[i].x) / settings.Scale * frequency;
                    var sampleY = (y - halfHeight + octaveOffsets[i].y) / settings.Scale * frequency;

                    var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= settings.Persistence;
                    frequency *= settings.Lacunarity;
                }

                if (noiseHeight > maxLocalNoiseHeight) maxLocalNoiseHeight = noiseHeight;

                if (noiseHeight < minLocalNoiseHeight) minLocalNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;

                if (settings.NormalizeMode != NormalizeMode.Global) continue;
                var normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
            }

            if (settings.NormalizeMode == NormalizeMode.Local)
                for (var y = 0; y < mapHeight; y++)
                for (var x = 0; x < mapWidth; x++)
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);

            return new NoiseMap(noiseMap);
        }

        public static float GetNoise(float x, float y)
        {
            return Mathf.PerlinNoise(x, y);
        }
    }

    public class NoiseMap
    {
        private static float _globalMaxValue = 1;

        public float[,] HeightMap;

        public NoiseMap(float[,] heightMap)
        {
            HeightMap = heightMap;
        }

        public NoiseMap Add(Func<float, float> applier)
        {
            for (var i = 0; i < HeightMap.GetLength(0); i++)
            for (var j = 0; j < HeightMap.GetLength(1); j++)
                HeightMap[i, j] += applier(HeightMap[i, j]);
            return this;
        }

        public NoiseMap NormaliseMap()
        {
            var maxValue = GetMaxValue();

            for (var i = 0; i < HeightMap.GetLength(0); i++)
            for (var j = 0; j < HeightMap.GetLength(1); j++)
                HeightMap[i, j] /= maxValue;

            return this;
        }

        private float GetMaxValue()
        {
            _globalMaxValue = Mathf.Max(_globalMaxValue, HeightMap.Cast<float>().Max());
            return _globalMaxValue;
        }
    }

    public enum NormalizeMode
    {
        Local,
        Global
    }
}