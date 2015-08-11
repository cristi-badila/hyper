using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace HyperIoC.Mvc
{
    /// <summary>
    /// Defines a controller factory for resolving IController instances.
    /// </summary>
    public class HyperIoCControllerFactory : DefaultControllerFactory
    {
        private readonly IFactoryResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="resolver">IoC resolver</param>
        public HyperIoCControllerFactory(IFactoryResolver resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));

            _resolver = resolver;
        }

        /// <summary>
        /// Creates the controller from the IoC.
        /// </summary>
        /// <param name="requestContext">Request context</param>
        /// <param name="controllerName">Name of the controller</param>
        /// <returns>Controller instance</returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return _resolver.Get<IController>(controllerName + "Controller");
        }
    }
}