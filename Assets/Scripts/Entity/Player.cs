using System.Collections;
using System.Linq;
using Assets.Scripts.Helper;
using Assets.Scripts.World;
using Assets.Scripts.World.Chunks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private World.World _world;

        private Rigidbody2D _rigidbody;
        
        [SerializeField] private float _disposalDelay;
        [SerializeField] private float _editorDisposeDistance;
        [SerializeField] private float _editorViewDistance;
        [SerializeField] private float _generationDelay;
        [SerializeField] private float _speed;
        private float _disposeDistance;
        private float _viewDistance;

        private int _vertical, _horizontal;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _viewDistance = Mathf.Min(_editorViewDistance, _editorDisposeDistance);
            _disposeDistance = Mathf.Max(_editorDisposeDistance, _editorViewDistance);

            StartCoroutine(DisposeUnusedChunks());
            StartCoroutine(LoadWorld());
        }

        private void Update()
        {
            _vertical = _horizontal == 0 ? VerInput() : 0;
            _horizontal = _vertical == 0 ? HorInput() : 0;

            _rigidbody.position += new Vector2(_horizontal, _vertical) * _speed * Time.deltaTime;
            _animator.SetInteger("horizontal", _horizontal);
            _animator.SetInteger("vertical", _vertical);
        }

        private IEnumerator LoadWorld()
        {
            Vector2[] oldChunks = { };

            while (true)
            {
                var center = new Vector2(Mathf.Floor(transform.position.x / Chunk.ChunkSize),
                    Mathf.Floor(transform.position.y / Chunk.ChunkSize));

                var newChunks = center
                    .PointsInCircle(_viewDistance, 1)
                    .ToArray();
                foreach (var hide in oldChunks.Except(newChunks))
                    _world.GetChunk((int) hide.x, (int) hide.y)?.HideChunk();

                foreach (var chunk in newChunks)
                    _world.GetChunk((int) chunk.x, (int) chunk.y)?.ShowChunk();

                oldChunks = newChunks;
                yield return new WaitForSeconds(_generationDelay);
            }
        }

        private IEnumerator DisposeUnusedChunks()
        {
            while (true)
            {
                yield return new WaitForSeconds(_disposalDelay);
                foreach (var dispose in _world.GetLoadedChunks().ToArray()
                    .Select(i => i.GetOffset())
                    .Except(new Vector2(Mathf.Floor(transform.position.x / Chunk.ChunkSize),
                            Mathf.Floor(transform.position.y / Chunk.ChunkSize))
                        .PointsInCircle(_disposeDistance, 1)))
                    _world.UnloadChunk((int) dispose.x, (int) dispose.y);
            }
        }

        private static int VerInput()
        {
            if (Input.GetKey(KeyCode.W)) return 1;
            if (Input.GetKey(KeyCode.S)) return -1;
            return 0;
        }

        private static int HorInput()
        {
            if (Input.GetKey(KeyCode.D)) return 1;
            if (Input.GetKey(KeyCode.A)) return -1;
            return 0;
        }
    }
}