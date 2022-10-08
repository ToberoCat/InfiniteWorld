using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.World.Settings
{
    [Serializable]
    public class TileSettings
    {
        public TileHeight[] LandMassTiles;

        public Tile GetTile(float value)
        {
            return (from item in LandMassTiles where value <= item.Height select item.Tile).FirstOrDefault();
        }

        public void ValidateValues()
        {
            if (LandMassTiles == null) return;

            Array.Sort(LandMassTiles, (a, b) =>
                Comparer<double>.Default.Compare(a.Height, b.Height));
            foreach (var t in LandMassTiles)
            {
                var tile = t;
                tile.Name = tile.Tile?.name;
            }
        }
    }

    [Serializable]
    public struct TileHeight
    {
        public string Name;
        public Tile Tile;
        public double Height;
    }
}