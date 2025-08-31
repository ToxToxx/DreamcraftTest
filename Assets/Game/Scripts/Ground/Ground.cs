using UnityEngine;
using Game.Core.Di;
using Game.Gameplay.Player;

namespace Game.Gameplay.Ground
{
    [AddComponentMenu("Game/Ground/Ground")]
    public sealed class Ground : MonoBehaviour
    {
        [SerializeField] private Transform _teleportTarget;
        [SerializeField] private bool _useColliderBounds = true;


        [Inject] private PlayerController _playerRef;

        private Transform _player;
        private Bounds _bounds;
        private bool _hasBounds;

        void Awake()
        {
            if (_useColliderBounds)
            {
                var col = GetComponent<Collider>();
                if (col) { _bounds = col.bounds; _hasBounds = true; }
                else
                {
                    var rend = GetComponent<Renderer>();
                    if (rend) { _bounds = rend.bounds; _hasBounds = true; }
                }
            }
        }

        void OnEnable()
        {
            _player = _playerRef ? _playerRef.transform : null;

            if (_player == null && ServiceLocator.Container != null)
            {
                try { _player = ServiceLocator.Container
                        .Resolve<PlayerController>()?.transform; } catch { }
            }

            if (_player == null)
            {
                var p = FindFirstObjectByType<PlayerController>();
                if (p) _player = p.transform;
            }

            if (!_hasBounds)
            {
                _bounds = new Bounds(transform.position, Vector3.one * 100000f);
                _hasBounds = true;
            }
        }

        void Update()
        {
            if (_player == null)
            {
                var p = FindFirstObjectByType<PlayerController>();
                if (p) _player = p.transform;
                else return;
            }

            var pos = _player.position;
            var min = _bounds.min;
            var max = _bounds.max;

            bool outXZ = pos.x < min.x || pos.x > max.x || pos.z < min.z || pos.z > max.z;
            if (outXZ)
            {
                var t = _teleportTarget 
                    ? _teleportTarget.position 
                    : (_hasBounds ? _bounds.center : transform.position);
                _player.position = new Vector3(t.x, pos.y, t.z);
            }
        }
    }
}
