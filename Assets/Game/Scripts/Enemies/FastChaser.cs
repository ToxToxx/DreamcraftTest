namespace Game.Gameplay.Enemies
{
    public sealed class FastChaser : EnemyBase
    {
        protected override void Awake()
        {
            base.Awake();
            _maxHp = 2;
            _speed = 7.5f;
            _touchDamage = 1;
        }
    }
}