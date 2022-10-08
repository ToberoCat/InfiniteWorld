using System;
using UnityEngine;

namespace Assets.Scripts.World.Settings
{
    [CreateAssetMenu(fileName = "default-world", menuName = "Settings/World")]
    public class WorldSettings : ScriptableObject
    {
        public TerrainSettings TerrainSettings;
        public TileSettings TileSettings;
    }

    [Serializable]
    public struct TerrainSettings
    {
        public NoiseSettings NoiseSettings;
        public AnimationCurve HeightMultiplier;
    }
}