using UnityEngine;
using Game.Core.Di;
using Game.Core.Pool;
using Game.Configs;


namespace Game.Gameplay.Weapons
{
    public sealed class SpreadShotWeapon : IWeapon
    {
        private readonly WeaponConfig _cfg;
        private float _cd;
        private MonoPool<Bullet> _pool => (MonoPool<Bullet>)Game.Core.Di.ServiceLocator.Container.Resolve(typeof(MonoPool<Bullet>));


        public SpreadShotWeapon(WeaponConfig cfg) { _cfg = cfg; }


        public bool TryFire(Vector3 origin, Vector3 dir)
        {
            if (_cd > 0f) { _cd -= Time.deltaTime; return false; }
            _cd = _cfg.Cooldown;


            float half = _cfg.SpreadAngleDeg * 0.5f;
            for (int i = 0; i < _cfg.Pellets; i++)
            {
                float t = (i + 0.5f) / _cfg.Pellets; 
                float ang = Mathf.Lerp(-half, half, t);
                var q = Quaternion.AngleAxis(ang, Vector3.up);
                var v = q * dir * _cfg.BulletSpeed;
                var b = _pool.Spawn();
                b.transform.position = origin;
                b.Launch(v, _cfg.BulletLife, _cfg.Damage, x => _pool.Despawn(x));
            }
            return true;
        }
    }
}