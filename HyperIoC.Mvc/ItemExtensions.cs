namespace HyperIoC.Mvc
{
    /// <summary>
    /// Extends the item for lifetime management.
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Signals that this should be registered on a per-request basis.
        /// </summary>
        /// <param name="item">Item to register against</param>
        public static void AsPerRequest(this Item item)
        {
            item.SetLifetimeTo(new HttpContextLifetimeManager());
        }
    }
}
