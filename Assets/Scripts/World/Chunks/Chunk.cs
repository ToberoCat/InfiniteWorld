using System;
using Assets.Scripts.World.Generator;
using Assets.Scripts.World.Generator.Layer;
using UnityEngine;

namespace Assets.Scripts.World.Chunks
{
    public class Chunk : MonoBehaviour
    {
        public const int ChunkSize = 10;

        private object[] _dataProviders = new object[Enum.GetNames(typeof(ChunkLod)).Length];

        public bool IsVisible;

        public Action OnHideEvent;
        public Action OnShowEvent;
        public int X { get; set; }
        public int Y { get; set; }

        public void ShowChunk()
        {
            if (IsVisible) return;
            OnShowEvent?.Invoke();
            IsVisible = true;
        }

        public void HideChunk()
        {
            if (!IsVisible) return;
            OnHideEvent?.Invoke();
            IsVisible = false;
        }

        public void UnloadChunk()
        {
            OnHideEvent?.Invoke();
            _dataProviders = null;
            Destroy(gameObject);
        }

        public T GetData<T>(ChunkLod lod)
        {
            var rawData = _dataProviders[(int) lod];

            if (rawData is T data) return data;

            try
            {
                return (T) Convert.ChangeType(rawData, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        public Vector2Int GetIntOffset()
        {
            return new Vector2Int(ChunkSize * X, ChunkSize * Y);
        }

        public Vector2 GetOffset()
        {
            return new Vector2(ChunkSize * X, ChunkSize * Y);
        }

        public void AddDataProvider(IDataProvider<object> dataProvider)
        {
            _dataProviders[(int) dataProvider.GetLod()] = dataProvider.GetData(this);
        }

        public void AdaptData(IDataAdapter dataAdapter)
        {
            var lod = (int) dataAdapter.GetLod();
            _dataProviders[lod] = dataAdapter.ApplyLayer(this, _dataProviders[lod]);
        }
    }
}