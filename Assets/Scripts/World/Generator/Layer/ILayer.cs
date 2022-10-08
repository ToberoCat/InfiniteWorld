using Assets.Scripts.World.Chunks;

namespace Assets.Scripts.World.Generator.Layer
{
    public interface ILayer
    {
        ChunkLod GetLod();

        void Execute(Chunk chunk);
    }
}