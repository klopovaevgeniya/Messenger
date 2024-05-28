using System.Reflection;

namespace Messenger
{
    public class Singleton
    {
        private static readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();
        private static readonly object lockObject = new object();

        protected Singleton() { }

        public static T Instance<T>() where T : class
        {
            lock (lockObject)
            {
                if (!instances.ContainsKey(typeof(T)))
                {
                    T instance = CreateInstance<T>();
                    instances[typeof(T)] = instance;
                }
                return (T)instances[typeof(T)];
            }
        }

        private static T CreateInstance<T>() where T : class
        {
            ConstructorInfo? constructor = typeof(T)
                .GetConstructor(
                    bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null);
            if (constructor == null)
            {
                throw new MissingMethodException($"Type {typeof(T).FullName} does not have a private parameterless constructor.");
            }

            return (T)constructor.Invoke(null);
        }
    }
}
