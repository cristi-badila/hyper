namespace HyperMock.Universal.Tests.Core
{
    using System.Linq;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Universal.Core;

    [TestClass]
    public class CallRecordingsTests
    {
        private CallRecordings _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new CallRecordings();
        }

        [TestMethod]
        public void FilterWithCallDescriptor_AtLeastOneCallRecordingExistsWhichMatches_ReturnsThem()
        {
            const string memberName = "testName";
            var matchers = new ParameterMatchersList();

            _subject.Add(new CallRecording("test123", new object[0]));
            _subject.Add(new CallRecording(memberName, new object[0]));
            _subject.Add(new CallRecording("test124", new object[0]));

            var matches = _subject.Filter(new CallDescriptor(memberName, matchers)).ToList();

            Assert.AreEqual(1, matches.Count);
            Assert.AreSame(matches[0], _subject[1]);
        }

        [TestMethod]
        public void FilterWithCallDescriptor_NoCallRecordingExistsWhichMatches_ReturnsNoRecordings()
        {
            const string memberName = "testName";
            var matchers = new ParameterMatchersList();

            _subject.Add(new CallRecording("test123", new object[0]));
            _subject.Add(new CallRecording("test124", new object[0]));

            var matches = _subject.Filter(new CallDescriptor(memberName, matchers)).ToList();

            Assert.AreEqual(0, matches.Count);
        }
    }
}
