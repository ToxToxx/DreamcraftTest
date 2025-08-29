using UnityEngine;
using Game.Core.Di;
using Game.Core.Pool;


namespace Game.Gameplay.Enemies
{
    [AddComponentMenu("Game/Enemies/Spawner 3D")]
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float _interval = 1.0f;
        [SerializeField] private float _minRadius = 22f;
        [SerializeField] private float _maxRadius = 30f;
        [SerializeField, Range(0f, 1f)] private float _slowChance = 0.35f;
        [SerializeField] private float _y = 0f; 


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


            bool slow = Random.value < _slowChance;
            if (slow)
            {
                var e = _slowPool.Spawn();
                SpawnEnemy(e);
                e.SetOnDie(x => _slowPool.Despawn((SlowTank)x));
            }
            else
            {
                var e = _fastPool.Spawn();
                SpawnEnemy(e);
                e.SetOnDie(x => _fastPool.Despawn((FastChaser)x));
            }
        }


        private void SpawnEnemy(EnemyBase e)
        {
            var pos = RandomPointOnRing();
            e.transform.position = pos;
            e.Init(_target);
        }


        private Vector3 RandomPointOnRing()
        {
            float r = Random.Range(_minRadius, _maxRadius);
            float a = Random.Range(0f, Mathf.PI * 2f);
            var offset = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * r;
            var p = _target.position + offset; p.y = _y;
            return p;
        }
    }
}