using UnityEngine;
using Game.Core.Di;
using Game.Core.Pool;
using Game.Gameplay.Enemies;
using Game.Gameplay.Player;
using Game.Gameplay.Session;


namespace Game.Gameplay.Bootstrap
{
    [AddComponentMenu("Game/Bootstrap/Game Root 3D")]
    public sealed class GameRoot : MonoBehaviour
    {
        [Header("Scene Prefabs")]
        [SerializeField] private Transform _groundPrefab; 
        [SerializeField] private PlayerController _playerPrefab;


        [Header("Weapons")]
        [SerializeField] private Game.Configs.WeaponConfig _singleShot;
        [SerializeField] private Game.Configs.WeaponConfig _spreadShot;


        [Header("Bullet")]
        [SerializeField] private Game.Gameplay.Weapons.Bullet _bulletPrefab;


        [Header("Enemies")]
        [SerializeField] private FastChaser _fastPrefab;
        [SerializeField] private SlowTank _slowPrefab;


        [Header("Misc")]
        [SerializeField] private Camera _camera;


        private IServiceResolver _di;


        private MonoPool<Game.Gameplay.Weapons.Bullet> _bulletPool;
        private MonoPool<FastChaser> _fastPool;
        private MonoPool<SlowTank> _slowPool;


        void Awake()
        {
            _di = new Container();
            ServiceLocator.Set(_di);

            if (_groundPrefab) Instantiate(_groundPrefab);


            var poolsRoot = new GameObject("[Pools]").transform;


            _bulletPool = new MonoPool<Game.Gameplay.Weapons.Bullet>(_bulletPrefab, poolsRoot, _di, preload: 64);
            _fastPool = new MonoPool<FastChaser>(_fastPrefab, poolsRoot, _di, preload: 10);
            _slowPool = new MonoPool<SlowTank>(_slowPrefab, poolsRoot, _di, preload: 6);


            _di.BindInstance(_camera);
            _di.BindInstance(_bulletPool);
            _di.BindInstance(_fastPool);
            _di.BindInstance(_slowPool);


            var sessionGo = new GameObject("[GameSession]");
            var session = sessionGo.AddComponent<GameSession>();
            (_di as Container)!.Inject(session);
            _di.BindInstance(session);


            var player = Instantiate(_playerPrefab);
            (_di as Container)!.InjectGameObject(player.gameObject);


            var weapons = player.GetComponent<WeaponController>();
            weapons.SetupWeapons(_singleShot, _spreadShot);


            session.Attach(player.Health);


            var spawnerGo = new GameObject("[EnemySpawner]");
            var spawner = spawnerGo.AddComponent<EnemySpawner>();
            (_di as Container)!.Inject(spawner);
            spawner.Begin(player.transform);
        }
    }
}