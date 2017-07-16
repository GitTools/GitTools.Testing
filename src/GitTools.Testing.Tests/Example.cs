using Xunit;
using Xunit.Abstractions;

namespace GitTools.Testing.Tests
{
    using System;
#if NET452
    using System.Runtime.Remoting.Messaging;
#endif
    using JetBrains.Annotations;
    using Logging;

    public class Example
    {
        private readonly ITestOutputHelper _outputHelper;
        public Example(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }

        [Fact]
        public void TheReadmeSample()
        {
#if NET452
            using (LogHelper.Capture(_outputHelper, LogProvider.SetCurrentLogProvider))
            {
#endif
                using (var fixture = new EmptyRepositoryFixture())
                {
                    fixture.MakeACommit();
                    fixture.MakeACommit();
                    fixture.MakeATaggedCommit("1.0.0");
                    fixture.BranchTo("develop");
                    fixture.MakeACommit();
                    fixture.Checkout("master");
                    fixture.MergeNoFF("develop");
                }
#if NET452
            }
#endif
        }
    }

#if NET452
    public static class LogHelper
    {
        private static readonly XUnitProvider Provider;

        static LogHelper()
        {
            Provider = new XUnitProvider();
        }

        public static IDisposable Capture(ITestOutputHelper outputHelper, Action<ILogProvider> setProvider)
        {
            // TODO Only do this once
            setProvider(Provider);

            CallContext.SetData("CurrentOutputHelper", outputHelper);

            return new DelegateDisposable(() =>
            {
                CallContext.SetData("CurrentOutputHelper", null);
            });
        }

        class DelegateDisposable : IDisposable
        {
            private readonly Action _action;

            public DelegateDisposable(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }

    public class XUnitProvider : ILogProvider
    {
        public Logger GetLogger(string name)
        {
            return XUnitLogger;
        }

        private bool XUnitLogger(LogLevel logLevel, [CanBeNull] Func<string> messageFunc, [CanBeNull] Exception exception, params object[] formatParameters)
        {
            if (messageFunc == null) return true;
            var currentHelper = (ITestOutputHelper)CallContext.GetData("CurrentOutputHelper");
            if (currentHelper == null)
                return false;

            currentHelper.WriteLine("[{0}] {1}", logLevel, messageFunc());
            if (exception != null)
                currentHelper.WriteLine("Exception:{0}{1}", Environment.NewLine, exception.ToString());

            return true;
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
#endif
}