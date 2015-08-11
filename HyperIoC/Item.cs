using System;
using System.Collections.Generic;

namespace HyperIoC
{
    /// <summary>
    /// Defines a IoC registered item. All items are registered as transient by default.
    /// </summary>
    public class Item
    {
        private ItemDetail _currentTypeDetail;

        internal Item(Type interfaceType)
        {
            InstanceTypes = new Dictionary<string, ItemDetail>();
            InterfaceType = interfaceType;
        }

        internal Type InterfaceType { get; }

        internal Dictionary<string, ItemDetail> InstanceTypes { get; }

        /// <summary>
        /// Signals that this item should be registered as a singleton.
        /// </summary>
        public void AsSingleton()
        {
            _currentTypeDetail.LifetimeManager = new SingletonLifetimeManager();
        }

        internal void AddType(string key, Type type)
        {
            _currentTypeDetail = new ItemDetail(type);
            InstanceTypes.Add(key, _currentTypeDetail);
        }
    }
}