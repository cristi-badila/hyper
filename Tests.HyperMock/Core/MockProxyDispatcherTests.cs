namespace HyperMock.Universal.Tests.Core
{
    using System;
    using System.Reflection;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using ParameterMatchers;
    using Support;
    using Universal.Core;

    [TestClass]
    public class MockProxyDispatcherTests
    {
        private MockProxyDispatcher _subject;
        private ITestInterface _mockImplementation;

        [TestInitialize]
        public void Setup()
        {
            _mockImplementation = DispatchProxy.Create<ITestInterface, MockProxyDispatcher>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            _subject = _mockImplementation as MockProxyDispatcher;
        }

        [TestMethod]
        public void Constructor_Always_InitializesKnownCallDescriptors()
        {
            Assert.IsNotNull(new MockProxyDispatcher().KnownCallDescriptors);
        }

        [TestMethod]
        public void Constructor_Always_InitializesRecordedCalls()
        {
            Assert.IsNotNull(new MockProxyDispatcher().RecordedCalls);
        }

        [TestMethod]
        public void Invoke_AMethodWithoutArgsIsCalled_RecordsCall()
        {
            _mockImplementation.Action1();

            Assert.AreEqual(1, _subject.RecordedCalls.Count);
            Assert.AreEqual("Action1", _subject.RecordedCalls[0].MemberName);
            CollectionAssert.AreEquivalent(new object[0], _subject.RecordedCalls[0].Arguments);
        }

        [TestMethod]
        public void Invoke_AMethodWithASingleArgIsCalled_RecordsCall()
        {
            _mockImplementation.Action2(1);

            Assert.AreEqual(1, _subject.RecordedCalls.Count);
            Assert.AreEqual("Action2", _subject.RecordedCalls[0].MemberName);
            CollectionAssert.AreEquivalent(new object[] { 1 }, _subject.RecordedCalls[0].Arguments);
        }

        [TestMethod]
        public void Invoke_AMethodWithMultipleArgsIsCalled_RecordsCall()
        {
            var objectParam = new object();
            _mockImplementation.Action3(2, objectParam);

            Assert.AreEqual(1, _subject.RecordedCalls.Count);
            Assert.AreEqual("Action3", _subject.RecordedCalls[0].MemberName);
            CollectionAssert.AreEquivalent(new[] { 2, objectParam }, _subject.RecordedCalls[0].Arguments);
        }

        [TestMethod]
        public void Invoke_APropertyIsRead_RecordsCall()
        {
            // ReSharper disable once UnusedVariable
            var value1 = _mockImplementation.Prop1;

            Assert.AreEqual(1, _subject.RecordedCalls.Count);
            Assert.AreEqual("get_Prop1", _subject.RecordedCalls[0].MemberName);
            CollectionAssert.AreEquivalent(new object[0], _subject.RecordedCalls[0].Arguments);
        }

        [TestMethod]
        public void Invoke_APropertyIsSet_RecordsCall()
        {
            _mockImplementation.Prop3 = 5;

            Assert.AreEqual(1, _subject.RecordedCalls.Count);
            Assert.AreEqual("set_Prop3", _subject.RecordedCalls[0].MemberName);
            CollectionAssert.AreEquivalent(new object[] { 5 }, _subject.RecordedCalls[0].Arguments);
        }

        [TestMethod]
        public void Invoke_NoKnownCallDescriptorsMatchAndFuncReturnsAValueType_ReturnsTheDefaultValueForTheType()
        {
            Assert.AreEqual(0, _mockImplementation.Function1());
        }

        [TestMethod]
        public void Invoke_NoKnownCallDescriptorsMatchAndFuncReturnsAClassType_ReturnsNull()
        {
            Assert.AreEqual(null, _mockImplementation.Function2());
        }

        [TestMethod]
        public void Invoke_AKnownCallDescriptorMatches_ReturnsTheDescriptorReturnValue()
        {
            var callDescriptor = new CallDescriptor
            {
                MemberName = "Function3",
                ParameterMatchers = new ParameterMatchersList { new AnyMatcher() },
                ReturnValue = 13
            };
            _subject.KnownCallDescriptors.Add(callDescriptor);

            Assert.AreEqual(13, _mockImplementation.Function3(1));
        }

        [TestMethod]
        public void Invoke_AKnownCallDescriptorMatchesAndHasAnExceptionTypeSet_ThrowsTheException()
        {
            var callDescriptor = new CallDescriptor
            {
                MemberName = "Function3",
                ParameterMatchers = new ParameterMatchersList { new AnyMatcher() },
                ReturnValue = 13,
                ExceptionType = typeof(CustomException)
            };
            _subject.KnownCallDescriptors.Add(callDescriptor);

            Assert.ThrowsException<CustomException>(() => _mockImplementation.Function3(1));
        }

        [TestMethod]
        public void Invoke_MoreThenOneKnownCallDescriptorMatch_ReturnsTheDescriptorReturnValueOfTheLastMatchingDescriptor()
        {
            var callDescriptor1 = new CallDescriptor
            {
                MemberName = "Function3",
                ParameterMatchers = new ParameterMatchersList { new ExactMatcher(2) },
                ReturnValue = 13
            };
            _subject.KnownCallDescriptors.Add(callDescriptor1);
            var callDescriptor2 = new CallDescriptor
            {
                MemberName = "Function3",
                ParameterMatchers = new ParameterMatchersList { new ExactMatcher(3) },
                ReturnValue = 14
            };
            _subject.KnownCallDescriptors.Add(callDescriptor2);
            var callDescriptor3 = new CallDescriptor
            {
                MemberName = "Function3",
                ParameterMatchers = new ParameterMatchersList { new ExactMatcher(2) },
                ReturnValue = 15
            };
            _subject.KnownCallDescriptors.Add(callDescriptor3);

            Assert.AreEqual(15, _mockImplementation.Function3(2));
        }
    }
}
