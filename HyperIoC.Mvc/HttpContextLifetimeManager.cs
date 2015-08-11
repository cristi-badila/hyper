using System;
using System.Web;

namespace HyperIoC.Mvc
{
    /// <summary>
    /// Defines a HTTP context lifetime manager.
    /// </summary>
    public class HttpContextLifetimeManager : LifetimeManager
    {
        public override object Get(Type type, IFactoryLocator locator, IFactoryResolver resolver)
        {
            var context = HttpContext.Current;

            if (context == null)
                throw new InvalidOperationException("The HTTP context is not available.");

            lock (context.Items.SyncRoot)
            {
                var key = "HyperIoC-" + type.FullName;
                context.Items[key] = context.Items[key] ?? CreateInstance(type, locator, resolver);
                return context.Items[key];
            }
        }
    }
}