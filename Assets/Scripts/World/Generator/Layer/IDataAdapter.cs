namespace Assets.Scripts.World.Generator.Layer
{
    public interface IDataAdapter : ILayer
    {
        ChunkLod ILayer.GetLod()
        {
            return GetAdaptedLayer();
        }

        ChunkLod GetAdaptedLayer();
        object ApplyLayer(Chunk chunk, object layer);
    }

    public interface IDataAdapter<T> : IDataAdapter
    {
        object IDataAdapter.ApplyLayer(Chunk chunk, object layer)
        {
            return ApplyLayer(chunk, (T) layer);
        }

        void ILayer.Execute(Chunk chunk)
        {
        }

        T ApplyLayer(Chunk chunk, T layer);
    }
}