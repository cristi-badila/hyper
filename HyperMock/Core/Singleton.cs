namespace HyperMock.Universal.Core
{
    using System;

    public abstract class Singleton<T>
        where T : class, new()
    {
        private static readonly Lazy<T> Lazy = new Lazy<T>(() => (T)Activator.CreateInstance(typeof(T)));

        public static T Instance => Lazy.Value;
    }
}