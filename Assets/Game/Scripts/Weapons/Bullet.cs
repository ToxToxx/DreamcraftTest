using UnityEngine;
using Game.Core.Pool;

namespace Game.Gameplay.Weapons
{
    [RequireComponent(typeof(Collider))]
    public sealed class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private SphereCollider _col;
        [SerializeField] private LayerMask _hitMask = ~0;
        private float _life;
        private float _lifeMax;
        private int _damage;
        private System.Action<Bullet> _onDone;
        private Vector3 _prevPos;

        public void Launch(Vector3 vel, float life, int damage, System.Action<Bullet> onDone)
        {
            _damage = damage;
            _life = 0f;
            _lifeMax = life;
            _onDone = onDone;
            _rb.linearVelocity = vel;
            _prevPos = transform.position;
        }

        void Update()
        {
            var cur = transform.position;
            var delta = cur - _prevPos;
            var dist = delta.magnitude;
            if (dist > 0f)
            {
                float radius = _col ? _col.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z) : 0.1f;
                if (Physics.SphereCast(_prevPos, radius, delta.normalized, out var hit, dist, _hitMask, QueryTriggerInteraction.Collide))
                {
                    var e = hit.collider.GetComponent<Game.Gameplay.Enemies.EnemyBase>() ?? hit.collider.GetComponentInParent<Game.Gameplay.Enemies.EnemyBase>();
                    if (e != null)
                    {
                        e.TakeDamage(_damage);
                        _onDone?.Invoke(this);
                        return;
                    }
                }
            }
            _prevPos = cur;

            _life += Time.deltaTime;
            if (_life >= _lifeMax)
            {
                _onDone?.Invoke(this);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var e = other.GetComponent<Game.Gameplay.Enemies.EnemyBase>() ?? other.GetComponentInParent<Game.Gameplay.Enemies.EnemyBase>();
            if (e != null)
            {
                e.TakeDamage(_damage);
                _onDone?.Invoke(this);
            }
        }

        public void OnSpawned()
        {
            _prevPos = transform.position;
        }

        public void OnDespawned()
        {
            if (_rb)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

        }
    }
}
