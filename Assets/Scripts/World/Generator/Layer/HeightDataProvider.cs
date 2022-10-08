using Assets.Scripts.ProcGen.Noise;
using Assets.Scripts.World.Settings;
using UnityEngine;

namespace Assets.Scripts.World.Generator.Layer
{
    public class HeightDataProvider : IDataProvider<NoiseMap>
    {
        private readonly NoiseSettings _settings;

        public HeightDataProvider(NoiseSettings settings)
        {
            _settings = settings;
        }

        public ChunkLod GetLod()
        {
            return ChunkLod.HeightMap;
        }

        public NoiseMap GetData(Chunk chunk)
        {
            return _settings.GenerateNoiseMap(Chunk.ChunkSize, Chunk.ChunkSize,
                new Vector2(Chunk.ChunkSize * chunk.X, Chunk.ChunkSize * chunk.Y));
        }
    }
}