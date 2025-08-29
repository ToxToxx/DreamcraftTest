namespace Game.Core.Di
{
    public static class ServiceLocator
    {
        public static IServiceResolver Container { get; private set; }
        public static void Set(IServiceResolver container) => Container = container;
    }
}