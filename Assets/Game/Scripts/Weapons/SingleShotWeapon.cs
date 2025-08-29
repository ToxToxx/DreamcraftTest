using UnityEngine;
using Game.Core.Di;
using Game.Core.Pool;
using Game.Configs;

namespace Game.Gameplay.Weapons
{
    public sealed class SingleShotWeapon : IWeapon
    {
        private readonly WeaponConfig _cfg;
        private float _cd;
        private MonoPool<Bullet> _pool => (MonoPool<Bullet>)ServiceLocator.Container.Resolve(typeof(MonoPool<Bullet>));

        public SingleShotWeapon(WeaponConfig cfg) { _cfg = cfg; }

        public bool TryFire(Vector3 origin, Vector3 dir)
        {
            if (_cd > 0f) { _cd -= Time.deltaTime; return false; }
            _cd = _cfg.Cooldown;

            _pool.Spawn(b =>
            {
                b.transform.SetPositionAndRotation(origin, Quaternion.LookRotation(dir, Vector3.up));
                var angle = Quaternion.AngleAxis(_cfg.MuzzleSpreadDeg * (Random.value - 0.5f), Vector3.up);
                var v = angle * dir * _cfg.BulletSpeed;
                b.Launch(v, _cfg.BulletLife, _cfg.Damage, x => _pool.Despawn(x));
            });

            return true;
        }
    }
}
