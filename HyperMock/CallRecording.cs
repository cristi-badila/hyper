namespace HyperMock.Universal
{
    public class CallRecording
    {
        public CallRecording(string name, object[] args)
        {
            MemberName = name;
            Arguments = args;
        }

        public string MemberName { get; set; }

        public object[] Arguments { get; set; }
    }
}