using UnityEngine;
using Game.Core.Pool;
using Game.Gameplay.Player;

namespace Game.Gameplay.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class EnemyBase : MonoBehaviour, IPoolable, IEnemy
    {
        [SerializeField] protected int _maxHp = 3;
        [SerializeField] protected float _speed = 4f;
        [SerializeField] protected int _touchDamage = 1;

        protected int _hp;
        protected Transform _target;
        protected Rigidbody _rb;
        protected System.Action<EnemyBase> _onDie;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        public void Init(Transform target) { _target = target; _hp = _maxHp; }

        protected virtual void Update()
        {
            if (!_target) return;
            var to = _target.position - transform.position; to.y = 0f;
            var dir = to.sqrMagnitude > 0.0001f ? to.normalized : Vector3.zero;
            _rb.linearVelocity = dir * _speed + Vector3.up * _rb.linearVelocity.y;
            if (dir.sqrMagnitude > 0.0001f) transform.forward = dir;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerHealth>(out var ph))
                ph.TakeDamage(_touchDamage);
        }

        public void TakeDamage(int dmg)
        {
            _hp -= Mathf.Abs(dmg);
            if (_hp <= 0) _onDie?.Invoke(this);
        }

        public void SetOnDie(System.Action<EnemyBase> cb) => _onDie = cb;

        public virtual void OnSpawned()
        {
            _hp = _maxHp;
            if (_rb) { _rb.linearVelocity = Vector3.zero; _rb.angularVelocity = Vector3.zero; }
        }

        public virtual void OnDespawned()
        {
            if (_rb) { _rb.linearVelocity = Vector3.zero; _rb.angularVelocity = Vector3.zero; }
            _target = null;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
