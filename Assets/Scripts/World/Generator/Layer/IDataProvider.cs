namespace Assets.Scripts.World.Generator.Layer
{
    public interface IDataProvider<out T> : ILayer
    {
        void ILayer.Execute(Chunk chunk)
        {
        }

        T GetData(Chunk chunk);
    }
}