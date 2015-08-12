using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace HyperIoC.Wcf
{
    /// <summary>
    /// Defines HyperIoCInstanceProvider class.
    /// </summary>
    internal class HyperIoCInstanceProvider : IInstanceProvider
    {
        private readonly IFactoryResolver _resolver;
        private readonly Type _contractType;

        /// <summary>
        /// Initialises a new instance of the class.
        /// </summary>
        internal HyperIoCInstanceProvider(IFactoryResolver resolver, Type contractType)
        {
            _resolver = resolver;
            _contractType = contractType;
        }

        /// <summary>
        /// Gets the instance from the IoC for the required contract type.
        /// </summary>
        /// <param name="instanceContext">Instance context</param>
        /// <param name="message">Service message</param>
        /// <returns>Instance if found else null</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var instance = _resolver.Get(_contractType);
            return instance;
        }

        /// <summary>
        /// Gets the instance from the IoC for the required contract type.
        /// </summary>
        /// <param name="instanceContext">Instance context</param>
        public object GetInstance(InstanceContext instanceContext)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            return GetInstance(instanceContext, null);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Releases the servuce instance. Nothing required to perform here. leave it for
        /// the GC.
        /// </summary>
        /// <param name="instanceContext">Instance context</param>
        /// <param name="instance">Instance to release</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {

        }
    }
}
