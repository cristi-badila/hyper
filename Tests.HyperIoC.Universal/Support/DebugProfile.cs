using HyperIoC.Universal;

namespace Tests.HyperIoC.Universal.Support
{
    public class DebugProfile : FactoryProfile
    {
        public override void Construct(IFactoryBuilder builder)
        {
            builder.Add<ITestClass, TestClass>();
        }
    }
}