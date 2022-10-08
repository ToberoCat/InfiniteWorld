using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Biome
{
    [CreateAssetMenu(menuName = "World/Tile", fileName = "tile")]
    public class VariationTile : Tile
    {
        public Sprite[] Variants;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            if (Variants.Length == 0) return;

            tileData.sprite = Variants[Random.Range(0, Variants.Length)];
        }
    }
}