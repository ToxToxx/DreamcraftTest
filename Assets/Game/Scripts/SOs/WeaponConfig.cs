using UnityEngine;


namespace Game.Configs
{
    [CreateAssetMenu(menuName = "Game/Weapon Config", fileName = "WeaponConfig")]
    public sealed class WeaponConfig : ScriptableObject
    {
        [Header("Common")]
        public float Cooldown = 0.15f;
        public int Damage = 1;
        public float BulletSpeed = 28f;
        public float BulletLife = 2.5f;
        public float MuzzleSpreadDeg = 0f; 


        [Header("Spread (if used)")]
        public int Pellets = 6;
        public float SpreadAngleDeg = 22f; 
    }
}