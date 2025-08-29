using UnityEngine;
using Game.Core.Di;
using Game.Gameplay.Inputs;


namespace Game.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu("Game/Player/Player Controller 3D")]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float _speed = 9f;


        [Header("Aim")]
        [SerializeField] private LayerMask _groundMask; 
        [SerializeField] private float _aimMaxDist = 500f;


        [Header("Refs")]
        [SerializeField] private PlayerHealth _health;


        [Inject] private Camera _cam;
        private IInputSource _input;
        private Rigidbody _rb;
        private Vector2 _move;
        private Vector3 _aimPoint;


        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // keep upright
#if UNITY_ANDROID || UNITY_IOS
_input = new MobileInputSource();
#else
            _input = new PcInputSource();
#endif
        }


        void Update()
        {
            _move = _input.Move;
            _aimPoint = _input.AimPoint(_cam, _groundMask, _aimMaxDist);

            Vector3 dir = _aimPoint - transform.position; dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
                transform.forward = dir.normalized;
        }


        void FixedUpdate()
        {
            var vel = new Vector3(_move.x, 0f, _move.y) * _speed;
            _rb.linearVelocity = vel + Vector3.up * _rb.linearVelocity.y; 
        }


        public IInputSource InputSource => _input;
        public PlayerHealth Health => _health;
    }
}