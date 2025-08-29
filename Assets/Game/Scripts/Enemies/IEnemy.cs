namespace Game.Gameplay.Enemies
{
    public interface IEnemy
    {
        void Init(UnityEngine.Transform target);
        void TakeDamage(int dmg);
    }
}