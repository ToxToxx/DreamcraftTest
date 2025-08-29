using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace Game.Core.Di
{
    public sealed class Container : IServiceResolver
    {
        private readonly Dictionary<Type, object> _instances = new();
        private readonly Dictionary<Type, Func<IServiceResolver, object>> _factories = new();


        public IServiceResolver BindInstance<T>(T instance)
        {
            _instances[typeof(T)] = instance!;
            return this;
        }


        public IServiceResolver BindFactory<T>(Func<IServiceResolver, T> factory)
        {
            _factories[typeof(T)] = r => factory(r)!;
            return this;
        }


        public T Resolve<T>() => (T)Resolve(typeof(T));


        public object Resolve(Type t)
        {
            if (_instances.TryGetValue(t, out var inst)) return inst;
            if (_factories.TryGetValue(t, out var fac))
            {
                var obj = fac(this);
                _instances[t] = obj;
                return obj;
            }


            var ctor = t.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                var obj2 = ctor.Invoke(null);
                _instances[t] = obj2;
                Inject(obj2);
                return obj2;
            }


            throw new Exception($"[DI] Cannot resolve type {t.Name}");
        }


        public void Inject(object target)
        {
            if (target == null) return;
            var type = target.GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;


            foreach (var f in type.GetFields(flags))
            {
                if (Attribute.IsDefined(f, typeof(InjectAttribute)))
                {
                    var dep = Resolve(f.FieldType);
                    f.SetValue(target, dep);
                }
            }


            foreach (var p in type.GetProperties(flags))
            {
                if (Attribute.IsDefined(p, typeof(InjectAttribute)) && p.CanWrite)
                {
                    var dep = Resolve(p.PropertyType);
                    p.SetValue(target, dep);
                }
            }
        }


        public void InjectGameObject(GameObject go)
        {
            var comps = go.GetComponentsInChildren<Component>(true);
            foreach (var c in comps)
            {
                if (c == null) continue; // missing script safety
                Inject(c);
            }
        }
    }
}