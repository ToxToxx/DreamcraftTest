using UnityEngine;


namespace Game.Gameplay.Weapons
{
    public interface IWeapon
    {
        bool TryFire(Vector3 origin, Vector3 dir);
    }
}