namespace Game.Gameplay.Enemies
{
    public sealed class SlowTank : EnemyBase
    {
        protected override void Awake()
        {
            base.Awake();
            _maxHp = 7;
            _speed = 3.2f;
            _touchDamage = 2;
        }
    }
}