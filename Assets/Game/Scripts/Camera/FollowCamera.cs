using UnityEngine;
using Game.Core.Di;
using Game.Gameplay.Player;

namespace Game.CameraSystem
{
    [AddComponentMenu("Game/Camera/Follow Player 3D (DI)")]
    public sealed class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = new Vector3(0f, 20f, -18f);
        [SerializeField] private float _smooth = 10f;
        [SerializeField] private bool _lookAt = true;

        [Inject] private PlayerController _player;
        private Transform _target;

        void OnEnable()
        {
            TryResolve();
        }

        void LateUpdate()
        {
            if (_target == null)
            {
                TryResolve();
                if (_target == null) return;
            }

            float a = 1f - Mathf.Exp(-_smooth * Time.deltaTime);
            var desired = _target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desired, a);

            if (_lookAt)
            {
                var dir = _target.position - transform.position;
                if (dir.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(dir.normalized, Vector3.up),
                        a
                    );
            }
        }

        void TryResolve()
        {
            if (_player != null) { _target = _player.transform; return; }

            var c = ServiceLocator.Container;
            if (c != null)
            {
                try
                {
                    var p = c.Resolve<PlayerController>();
                    if (p != null) { _target = p.transform; return; }
                }
                catch { }
            }

            var found = FindFirstObjectByType<PlayerController>();
            if (found) _target = found.transform;
        }
    }
}
