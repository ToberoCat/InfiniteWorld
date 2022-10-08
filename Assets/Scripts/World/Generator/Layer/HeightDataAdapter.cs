using Assets.Scripts.ProcGen.Noise;
using UnityEngine;

namespace Assets.Scripts.World.Generator.Layer
{
    public class HeightDataAdapter : IDataAdapter<NoiseMap>
    {
        private readonly AnimationCurve _heightMultiplier;

        public HeightDataAdapter(AnimationCurve heightMultiplier)
        {
            _heightMultiplier = heightMultiplier;
        }

        public ChunkLod GetAdaptedLayer()
        {
            return ChunkLod.HeightMap;
        }

        public NoiseMap ApplyLayer(Chunk chunk, NoiseMap heightMap)
        {
            return heightMap
                .Add(v => v * _heightMultiplier.Evaluate(v))
                .NormaliseMap();
        }
    }
}