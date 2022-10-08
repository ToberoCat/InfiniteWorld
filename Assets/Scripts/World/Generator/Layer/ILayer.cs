namespace Assets.Scripts.World.Generator.Layer
{
    public interface ILayer
    {
        ChunkLod GetLod();

        void Execute(Chunk chunk);
    }
}