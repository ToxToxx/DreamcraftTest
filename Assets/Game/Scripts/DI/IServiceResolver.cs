using System;


namespace Game.Core.Di
{
    public interface IServiceResolver
    {
        IServiceResolver BindInstance<T>(T instance);
        IServiceResolver BindFactory<T>(Func<IServiceResolver, T> factory);
        T Resolve<T>();
        object Resolve(Type t);
        void Inject(object target);
        void InjectGameObject(UnityEngine.GameObject go);
    }
}