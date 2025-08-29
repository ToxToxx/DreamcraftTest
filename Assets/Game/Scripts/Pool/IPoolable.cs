using UnityEngine;


namespace Game.Core.Pool
{
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}