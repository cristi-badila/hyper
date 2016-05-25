namespace HyperMock.Universal
{
    /// <summary>
    /// Mock helper for describing a parameter.
    /// </summary>
    public class Param
    {
        /// <summary>
        /// Indicates that any value of the type is allowed and will resolve.
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <returns></returns>
        public static T IsAny<T>()
        {
            return default(T);
        }
    }
}