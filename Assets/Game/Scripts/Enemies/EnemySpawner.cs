using UnityEngine;
using Game.Core.Di;
using Game.Core.Pool;

namespace Game.Gameplay.Enemies
{
    [AddComponentMenu("Game/Enemies/Spawner 3D")]
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float _interval = 1.0f;
        [SerializeField, Range(0f, 1f)] private float _slowChance = 0.35f;
        [SerializeField] private float _offscreenMargin = 0.08f;
        [SerializeField] private LayerMask _groundMask = ~0;
        [SerializeField] private float _rayMax = 500f;
        [SerializeField] private float _minDistanceFromPlayer = 12f;
        [SerializeField] private float _spawnY = 0f;

        [Inject] private Camera _cam;
        [Inject] private MonoPool<FastChaser> _fastPool;
        [Inject] private MonoPool<SlowTank> _slowPool;

        private Transform _target;
        private float _t;

        public void Begin(Transform target) { _target = target; }

        void Update()
        {
            if (!_target) return;
            _t += Time.deltaTime;
            if (_t < _interval) return;
            _t = 0f;

            if (Random.value < _slowChance)
            {
                _slowPool.Spawn(e => {
                    var p = NextOffscreenPoint(); p.y = _spawnY;
                    e.transform.position = p;
                    e.Init(_target);
                    e.SetOnDie(x => _slowPool.Despawn((SlowTank)x));
                });
            }
            else
            {
                _fastPool.Spawn(e => {
                    var p = NextOffscreenPoint(); p.y = _spawnY;
                    e.transform.position = p;
                    e.Init(_target);
                    e.SetOnDie(x => _fastPool.Despawn((FastChaser)x));
                });
            }
        }

        private Vector3 NextOffscreenPoint()
        {
            for (int i = 0; i < 6; i++)
            {
                float vx, vy;
                if (Random.value < 0.5f) 
                { vx = Random.value < 0.5f 
                        ? -_offscreenMargin 
                        : 1f + _offscreenMargin;
                    vy = Random.Range(0f, 1f); }
                else 
                { vy = Random.value < 0.5f 
                        ? -_offscreenMargin 
                        : 1f + _offscreenMargin; 
                    vx = Random.Range(0f, 1f); }

                var ray = _cam.ViewportPointToRay(new Vector3(vx, vy, 0f));
                if (Physics.Raycast(ray, out var hit, _rayMax, _groundMask, QueryTriggerInteraction.Ignore))
                {
                    var p = hit.point;
                    if ((p - _target.position).sqrMagnitude 
                        >= _minDistanceFromPlayer * _minDistanceFromPlayer)
                        return p;
                }
            }
            var f = _cam.transform.forward; f.y = 0f; f.Normalize();
            return _target.position + f * (_minDistanceFromPlayer + 5f);
        }
    }
}
