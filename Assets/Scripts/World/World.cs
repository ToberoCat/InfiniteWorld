using System.Collections.Generic;
using Assets.Scripts.Helper;
using Assets.Scripts.World.Chunks;
using Assets.Scripts.World.Generator;
using Assets.Scripts.World.Generator.Layer;
using Assets.Scripts.World.Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.World
{
    public class World : MonoBehaviour
    {
        private readonly Dictionary<(int, int), Chunk> _loadedChunks = new();
        private ChunkGenerator _generator;

        [SerializeField] private WorldSettings _settings;
        [SerializeField] private Tilemap _tilemap;


        private void Awake()
        {
            Random.InitState(0);

            _generator = new ChunkGenerator(transform, new ILayer[]
            {
                new HeightDataProvider(_settings.TerrainSettings.NoiseSettings),
                new HeightDataAdapter(_settings.TerrainSettings.HeightMultiplier),
                new RenderLayer(_tilemap, _settings.TileSettings)
            });
        }

        public IEnumerable<Chunk> GetLoadedChunks()
        {
            return _loadedChunks.Values;
        }

        public Chunk GetChunk(int x, int y)
        {
            return _loadedChunks.ComputeIfAbsent((x, y), _ => _generator.GenerateChunk(x, y, ChunkLod.Rendering));
        }

        public void UnloadChunk(int x, int y)
        {
            if (!_loadedChunks.TryGetValue((x, y), out var chunk)) return;
            chunk.UnloadChunk();
            _loadedChunks.Remove((x, y));
        }
    }
}