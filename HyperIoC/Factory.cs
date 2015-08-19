using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperIoC
{
    /// <summary>
    /// Provides registration of items in the factory for resolving via interfaces and abstract classes.
    /// </summary>
    public class Factory : IFactoryBuilder, IFactoryResolver, IFactoryLocator
    {
        private readonly List<Item> _items = new List<Item>();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Factory()
        {
            // Add self to the IoC (as resolver) for dependency resolution patterns.
            var item = new Item(typeof (IFactoryResolver));
            item.AddType(GetType().FullName, GetType());
            item.SetLifetimeTo(new SingletonLifetimeManager(this));
            _items.Add(item);
        }

        private IFactoryLocator Locator => (IFactoryLocator) this;

        /// <summary>
        /// Adds an item into the IoC.
        /// </summary>
        /// <typeparam name="TInterface">Interface to register</typeparam>
        /// <typeparam name="TType">Type to register with interface</typeparam>
        /// <param name="key">Optional key</param>
        /// <returns>Register item</returns>
        public Item Add<TInterface, TType>(string key = null) where TType : TInterface
        {
            return Add(typeof (TInterface), typeof (TType), key);
        }

        /// <summary>
        /// Adds an item into the IoC.
        /// </summary>
        /// <param name="interfaceType">Interface to register</param>
        /// <param name="instanceType">Type to register with interface</param>
        /// <param name="key">Optional key</param>
        /// <returns>Register item</returns>
        public Item Add(Type interfaceType, Type instanceType, string key = null)
        {
            CheckInterfaceType(interfaceType);
            CheckInstanceType(instanceType);

            var item = Locator.FindItem(interfaceType);

            key = key ?? instanceType.FullName;

            if (item == null) _items.Add(new Item(interfaceType));
            
            item = Locator.FindItem(interfaceType);

            if (!item.InstanceTypes.ContainsKey(key))
            {
                item.AddType(key, instanceType);
            }

            return item;
        }

        /// <summary>
        /// Gets an item from the IoC for the requesting interface.
        /// </summary>
        /// <typeparam name="TInterface">Interface to resolve type for</typeparam>
        /// <param name="key">Optional key</param>
        /// <returns>Instance if found else null</returns>
        public TInterface Get<TInterface>(string key = null)
        {
            var interfaceType = typeof(TInterface);
            return (TInterface)Get(interfaceType, key);
        }

        /// <summary>
        /// Gets an item from the IoC for the requesting interface.
        /// </summary>
        /// <param name="interfaceType">Interface to resolve type for</param>
        /// <param name="key">Optional key</param>
        /// <returns>Instance if found else null</returns>
        public object Get(Type interfaceType, string key = null)
        {
            CheckInterfaceType(interfaceType);

            var item = Locator.FindItem(interfaceType);

            if (item == null) return null;

            // If no key supplied then use the first one
            if (string.IsNullOrWhiteSpace(key))
            {
                key = item.InstanceTypes.Keys.First();
            }

            // If we have a type that matches the key then use this else return null
            return item.InstanceTypes.ContainsKey(key)
                ? item.InstanceTypes[key].LifetimeManager.Get(item.InstanceTypes[key].Type, this, this)
                : null;
        }

        /// <summary>
        /// Gets all the instances of the interface from the IoC.
        /// </summary>
        /// <param name="interfaceType">Interface to resolve types for</param>
        /// <returns>Resolved instances</returns>
        public object[] GetAll(Type interfaceType)
        {
            CheckInterfaceType(interfaceType);

            var item = Locator.FindItem(interfaceType);

            if (item == null) return null;

            var instanceTypes = new List<object>();

            foreach (var detail in item.InstanceTypes.Values)
            {
                var instance = detail.LifetimeManager.Get(detail.Type, this, this);
                instanceTypes.Add(instance);
            }

            return instanceTypes.ToArray();
        }

        /// <summary>
        /// Gets all the instances of the interface from the IoC.
        /// </summary>
        /// <typeparam name="TInterface">Interface to resolve type for</typeparam>
        /// <returns>Resolved instances</returns>
        public TInterface[] GetAll<TInterface>()
        {
            var instances = GetAll(typeof (TInterface)).ToList();
            var castInstances = new List<TInterface>();
            instances.ForEach(i => castInstances.Add((TInterface)i));
            return castInstances.ToArray();
        }

        /// <summary>
        /// Locates an item in the IoC registration list.
        /// </summary>
        /// <param name="interfaceType">Interface type to find</param>
        /// <returns>Item if found else null</returns>
        Item IFactoryLocator.FindItem(Type interfaceType)
        {
            return _items.FirstOrDefault(i => i.InterfaceType == interfaceType);
        }
        
        private static void CheckInterfaceType(Type interfaceType)
        {
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));
            if (!(interfaceType.IsInterface || interfaceType.IsAbstract))
                throw new ArgumentException("Type is not interface.", nameof(interfaceType));
        }

        private static void CheckInstanceType(Type instanceType)
        {
            if (instanceType == null) throw new ArgumentNullException(nameof(instanceType));
            if (instanceType.IsInterface || instanceType.IsAbstract)
                throw new ArgumentException("Instance is not a concerete type.");
        }
    }
}