using HyperIoC.Universal;

namespace Tests.HyperIoC.Universal.Support
{
    public class AnyProfile : FactoryProfile
    {
        public override void Construct(IFactoryBuilder builder)
        {
            builder.Add<ITestConfig, TestConfig>();
        }
    }
}