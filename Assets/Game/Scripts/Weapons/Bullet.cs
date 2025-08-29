using UnityEngine;
using Game.Core.Pool;


namespace Game.Gameplay.Weapons
{
    [RequireComponent(typeof(Collider))]
    public sealed class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] private Rigidbody _rb;
        private float _life;
        private float _lifeMax;
        private int _damage;
        private System.Action<Bullet> _onDone;


        public void Launch(Vector3 vel, float life, int damage, System.Action<Bullet> onDone)
        {
            _damage = damage;
            _life = 0f;
            _lifeMax = life;
            _onDone = onDone;
            _rb.linearVelocity = vel;
        }


        void Update()
        {
            _life += Time.deltaTime;
            if (_life >= _lifeMax)
                _onDone?.Invoke(this);
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Game.Gameplay.Enemies.EnemyBase>(out var e))
            {
                e.TakeDamage(_damage);
                _onDone?.Invoke(this);
            }
        }


        public void OnSpawned() { }
        public void OnDespawned() { if (_rb) _rb.linearVelocity = Vector3.zero; }
    }
}