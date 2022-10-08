using System;
using Assets.Scripts.ProcGen.Noise;
using UnityEngine;

namespace Assets.Scripts.World.Settings
{
    [Serializable]
    public class NoiseSettings
    {
        public float Lacunarity = 2;

        public NormalizeMode NormalizeMode;

        public int Octaves = 6;
        [Range(0, 1)] public float Persistence = .6f;
        public Vector2 SampleCentre;

        public float Scale = 50;

        public int Seed;

        public NoiseSettings(NormalizeMode normalizeMode, int seed, Vector2 sampleCentre)
        {
            NormalizeMode = normalizeMode;
            Seed = seed;
            SampleCentre = sampleCentre;
        }

        public void ValidateValues()
        {
            Scale = Mathf.Max(Scale, 0.01f);
            Octaves = Mathf.Max(Octaves, 1);
            Lacunarity = Mathf.Max(Lacunarity, 1);
            Persistence = Mathf.Clamp01(Persistence);
        }
    }
}