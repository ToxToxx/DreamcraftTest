using System.Collections.Generic;
using UnityEngine;
using Game.Core.Di;


namespace Game.Core.Pool
{
    public sealed class MonoPool<T> where T : Component, IPoolable
    {
        private readonly Stack<T> _stack = new();
        private readonly T _prefab;
        private readonly Transform _root;
        private readonly IServiceResolver _di;


        public MonoPool(T prefab, Transform root, IServiceResolver di, int preload = 0)
        {
            _prefab = prefab;
            _root = root;
            _di = di;
            for (int i = 0; i < preload; i++)
                Despawn(SpawnInternal());
        }


        private T SpawnInternal()
        {
            var inst = Object.Instantiate(_prefab, _root);
            (_di as Container)?.InjectGameObject(inst.gameObject);
            inst.gameObject.SetActive(false);
            return inst;
        }


        public T Spawn()
        {
            var obj = _stack.Count > 0 ? _stack.Pop() : SpawnInternal();
            obj.gameObject.SetActive(true);
            obj.OnSpawned();
            return obj;
        }

        public T Spawn(System.Action<T> setup)
        {
            var obj = _stack.Count > 0 ? _stack.Pop() : SpawnInternal();
            setup?.Invoke(obj);                     
            obj.gameObject.SetActive(true);         
            obj.OnSpawned();
            return obj;
        }


        public void Despawn(T obj)
        {
            obj.OnDespawned();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_root);
            _stack.Push(obj);
        }
    }
}