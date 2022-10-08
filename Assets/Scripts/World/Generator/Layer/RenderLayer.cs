using Assets.Scripts.ProcGen.Noise;
using Assets.Scripts.World.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.World.Generator.Layer
{
    public class RenderLayer : ILayer
    {
        private readonly TileSettings _settings;
        private readonly Tilemap _tilemap;

        public RenderLayer(Tilemap tilemap, TileSettings settings)
        {
            _tilemap = tilemap;
            _settings = settings;
        }

        public ChunkLod GetLod()
        {
            return ChunkLod.Rendering;
        }

        public void Execute(Chunk chunk)
        {
            var heightMap = chunk.GetData<NoiseMap>(ChunkLod.HeightMap).HeightMap;
            chunk.OnShowEvent += () =>
            {
                for (var i = 0; i < heightMap.GetLength(0); i++)
                for (var j = 0; j < heightMap.GetLength(1); j++)
                    _tilemap.SetTile(new Vector3Int(Chunk.ChunkSize * chunk.X + i,
                            Chunk.ChunkSize * chunk.Y + j, 0),
                        _settings.GetTile(heightMap[i, heightMap.GetLength(1) - j - 1]));
            };

            chunk.OnHideEvent += () =>
            {
                for (var i = 0; i < heightMap.GetLength(0); i++)
                for (var j = 0; j < heightMap.GetLength(1); j++)
                    _tilemap.SetTile(new Vector3Int(Chunk.ChunkSize * chunk.X + i,
                        Chunk.ChunkSize * chunk.Y + j, 0), null);
            };
        }
    }
}