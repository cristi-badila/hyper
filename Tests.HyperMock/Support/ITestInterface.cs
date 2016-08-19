namespace HyperMock.Universal.Tests.Core
{
    public interface ITestInterface
    {
        int Prop1 { get; set; }

        int Prop2 { get; }

        int Prop3 { set; }

        object Prop4 { get; set; }

        void Action1();

        void Action2(int p1);

        void Action3(int p1, object p2);

        int Function1();

        object Function2();

        int Function3(int p1);
    }
}