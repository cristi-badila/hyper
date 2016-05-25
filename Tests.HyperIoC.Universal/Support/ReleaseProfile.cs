using HyperIoC.Universal;

namespace Tests.HyperIoC.Universal.Support
{
    public class ReleaseProfile : FactoryProfile
    {
        public override void Construct(IFactoryBuilder builder)
        {
            builder.Add<ITestClass, AnotherTestClass>();
        }
    }
}