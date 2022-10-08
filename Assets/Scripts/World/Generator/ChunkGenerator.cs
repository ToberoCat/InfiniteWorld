using Assets.Scripts.World.Chunks;
using Assets.Scripts.World.Generator.Layer;
using UnityEngine;

namespace Assets.Scripts.World.Generator
{
    public class ChunkGenerator
    {
        private readonly ILayer[] _layers;
        private readonly Transform _parent;

        public ChunkGenerator(Transform parent, ILayer[] layers)
        {
            _layers = layers;
            _parent = parent;
        }

        public Chunk GenerateChunk(int x, int y, ChunkLod lod)
        {
            var chunkGm = new GameObject($"Chunk {x}, {y}")
            {
                transform =
                {
                    parent = _parent,
                    position = new Vector3(Chunk.ChunkSize * x, Chunk.ChunkSize * y)
                }
            };

            var chunk = chunkGm.AddComponent<Chunk>();
            chunk.X = x;
            chunk.Y = y;

            GenerateData(chunk, lod);
            return chunk;
        }

        private void GenerateData(Chunk chunk, ChunkLod lod)
        {
            ChunkLod current;

            var i = 0;
            do
            {
                var layer = _layers[i];
                current = layer.GetLod();

                switch (layer)
                {
                    case IDataProvider<object> provider:
                        chunk.AddDataProvider(provider);
                        break;
                    case IDataAdapter adapter:
                        chunk.AdaptData(adapter);
                        break;
                    default:
                        layer.Execute(chunk);
                        break;
                }

                i++;
            } while (current < lod);
        }
    }
}