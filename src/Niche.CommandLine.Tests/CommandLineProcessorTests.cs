using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineProcessorTests
    {

        private CommandLineProcessor<T> CreateProcessor<T>(params string[] arguments)
            where T : class
        {
            return new CommandLineProcessor<T>(arguments);
        }

        public class Constructor : CommandLineProcessorTests
        {
            [Fact]
            public void GivenNullForArguments_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineProcessor<BaseDriver>(null));
                exception.ParamName.Should().Be("arguments");
            }

            [Fact]
            public void WhenArgumentsSpecifyHelp_ConfiguresForHelp()
            {
                var processor = CreateProcessor<BaseDriver>("--help");
                processor.ShowHelp.Should().BeTrue();
            }

            [Fact]
            public void WhenArgumentsSpecifyHelp_RemovesArgumentFromList()
            {
                var processor = CreateProcessor<BaseDriver>("--help");
                processor.Arguments.Should().NotContain("--help");
            }

            /*
            [Fact]
            public void WithValidValueForParameter_ConfiguresDriver()
            {
                var arguments = new List<string> {"--repeat", "5"};
                var processor = new CommandLineProcessor<SampleDriver>(arguments, new SampleDriver());
                processor.Driver.Repeats.Should().Be(5);
            }

            [Fact]
            public void WithInvalidValueForParameter_GeneratesError()
            {
                var arguments = new List<string> {"--repeat", "twice"};
                var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
                processor.HasErrors.Should().BeTrue();
            }

            [Fact]
            public void SpecifyingMode_ReturnsDriverForMode()
            {
                var arguments = new List<string> {"test-performance", "--help"};
                var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
                processor.Driver.Should().BeOfType<TestDriver>();
            }

            [Fact]
            public void GivenAssignment_setsValue()
            {
                var arguments = new List<string>() { "--define", "Name=Donald" };
                var driver = new AssignmentDriver();
                var processor = new CommandLineProcessor<AssignmentDriver>(arguments, driver);
                driver["Name"].Should().Be("Donald");
            }

            */
        }

        public class ParseGlobalInstance : CommandLineProcessorTests
        {
            private readonly CommandLineProcessor _processor =
                CreateProcessor("--alpha", "beta", "gamma", "--verbose");

            [Fact]
            public void GivenNullOptions_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _processor.ParseGlobal<LoggingOptions>(null));
                exception.ParamName.Should().Be("options");
            }

            [Fact]
            public void GivenInstance_ConfiguresProperty()
            {
                var options = new LoggingOptions();
                _processor.ParseGlobal(options);
                options.IsVerbose.Should().BeTrue();
            }

            [Fact]
            public void GivenInstance_ReturnsExecutor()
            {
                var options = new LoggingOptions();
                _processor.ParseGlobal(options).Should().NotBeNull();
            }
        }

        public class ParseGlobalOfType : CommandLineProcessorTests
        {
            private readonly CommandLineProcessor _processor =
                CreateProcessor("--alpha", "beta", "gamma", "--verbose");

            [Fact]
            public void GivenInstance_ReturnsExecutor()
            {
                _processor.ParseGlobal<LoggingOptions>().Should().NotBeNull();
            }
            public void WithLongFormSwitch_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("--debug");
                var driver = new BaseDriver();
                processor.Configure(driver);
                driver.ShowDiagnostics.Should().BeTrue();
            }

            [Fact]
            public void WithShortFormSwitch_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("-d");
                var driver = new BaseDriver();
                processor.Configure(driver);
                driver.ShowDiagnostics.Should().BeTrue();
            }

            [Fact]
            public void WithAlternateShortFormSwitch_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("/d");
                var driver = new BaseDriver();
                processor.Configure(driver);
                driver.ShowDiagnostics.Should().BeTrue();
            }

            [Fact]
            public void WithLongFormParameter_CallsMethods()
            {
                var processor = CreateProcessor<BaseDriver>("--find", "file");
                var driver = new SampleDriver();
                processor.Configure(driver);
                driver.TextSearch.Should().Be("file");
            }

            [Fact]
            public void WithShortFormParameter_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("-f", "file");
                var driver = new SampleDriver();
                processor.Configure(driver);
                driver.TextSearch.Should().Be("file");
            }

            [Fact]
            public void WithAlternateShortFormParameter_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("/f", "file");
                var driver = new SampleDriver();
                processor.Configure(driver);
                driver.TextSearch.Should().Be("file");
            }

            [Fact]
            public void WithParameterRequiringConversion_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("/r", "4");
                var driver = new SampleDriver();
                processor.Configure(driver);
                driver.Repeats.Should().Be(4);
            }

            [Fact]
            public void WithUnexpectedArgument_LeavesItInArguments()
            {
                var processor = CreateProcessor<BaseDriver>("unexpected");
                var driver = new SampleDriver();
                processor.Configure(driver);
                processor.Arguments.Should().Contain("unexpected");
            }

            [Fact]
            public void WithUnexpectedOption_LeavesItInArguments()
            {
                var processor = CreateProcessor<BaseDriver>("--unexpected");
                var driver = new SampleDriver();
                processor.Configure(driver);
                processor.Arguments.Should().Contain("--unexpected");
            }
        }

        public class OptionHelp : CommandLineParameterTests
        {
            [Fact]
            public void ForValidDriver_ReturnsText()
            {
                var arguments = new List<string> { "test-performance", "--help" };
                var processor = new CommandLineProcessor<BaseDriver>(arguments);
                processor.Configure(new BaseDriver());
                processor.OptionHelp.Should().NotBeEmpty();
            }

            [Fact]
            public void WithNoOptions_ReturnsHelp()
            {
                var processor = new CommandLineProcessor<SampleDriver>(new List<string>());
                processor.Configure(new SampleDriver());
                processor.OptionHelp.Should().HaveCount(c => c > 0);
            }

            [Fact]
            public void WithKnownOptions_ReturnsHelp()
            {
                var processor = new CommandLineProcessor<SampleDriver>(new List<string>());
                processor.Configure(new SampleDriver());
                processor.OptionHelp.Should().HaveCount(8);
            }
        }

        public class Help : CommandLineParameterTests
        {
            [Fact]
            public void ForValidDriver_SetsShowHelp()
            {
                var arguments = new List<string> { "test-performance", "--help" };
                var processor = new CommandLineProcessor<BaseDriver>(arguments);
                processor.Configure(new BaseDriver());
                processor.ShowHelp.Should().BeTrue();
            }
        }

        public class Errors : CommandLineParameterTests
        {
            [Fact]
            public void Errors_ForInvalidParameter_ListsContent()
            {
                var arguments = new List<string> { "--not-an-option" };
                var processor = new CommandLineProcessor<BaseDriver>(arguments);
                processor.Process(new BaseDriver(), driver => { });
                processor.Errors.Should().NotBeEmpty();
            }
        }

        public class ProcessWithDriverAndAction : CommandLineProcessorTests
        {
            private BaseDriver _executedDriver;

            [Fact]
            public void GivenNullDriver_ThrowsExpectedException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.Process(null, Execute));
                exception.ParamName.Should().Be("driver");
            }

            [Fact]
            public void GivenNullAction_ThrowsExpectedException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var driver = new SampleDriver();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.Process(driver, (Action<BaseDriver>)null));
                exception.ParamName.Should().Be("action");
            }

            [Fact]
            public void WithValidArguments_CallsAction()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                _executedDriver.Should().Be(driver);
            }

            [Fact]
            public void WithExtraOption_DoesNotCallAction()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "--dingbat");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                _executedDriver.Should().BeNull();
            }

            [Fact]
            public void WithExtraOption_CreatesErrorForExtraOption()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "--dingbat");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                processor.Errors.Should().HaveCount(1);
                processor.Errors.Should().Contain(e => e.Contains("--dingbat"));
            }

            [Fact]
            public void WithExtraValue_DoesNotCallAction()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "value");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                _executedDriver.Should().BeNull();
            }

            [Fact]
            public void WithExtraValue_CreatesErrorForExtraValue()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "value");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                processor.Errors.Should().HaveCount(1);
                processor.Errors.Should().Contain(e => e.Contains("value"));
            }

            private void Execute(BaseDriver driver)
            {
                _executedDriver = driver;
            }
        }

        public class ProcessWithDriverAndActionAcceptingArguments : CommandLineProcessorTests
        {
            private BaseDriver _executedDriver;
            private List<string> _arguments;

            [Fact]
            public void GivenNullDriver_ThrowsExpectedException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.Process(null, Execute));
                exception.ParamName.Should().Be("driver");
            }

            [Fact]
            public void GivenNullAction_ThrowsExpectedException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var driver = new SampleDriver();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.Process(driver, (Action<BaseDriver>)null));
                exception.ParamName.Should().Be("action");
            }

            [Fact]
            public void WithValidArguments_CallsAction()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                _executedDriver.Should().Be(driver);
            }

            [Fact]
            public void WithExtraOption_DoesNotCallAction()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "--dingbat");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                _executedDriver.Should().BeNull();
            }

            [Fact]
            public void WithExtraOption_CreatesErrorForExtraOption()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "--dingbat");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                processor.Errors.Should().HaveCount(1);
                processor.Errors.Should().Contain(e => e.Contains("--dingbat"));
            }

            [Fact]
            public void WithExtraValue_CallsActionWithValue()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "value");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                _arguments.Should().BeEquivalentTo("value");
            }

            [Fact]
            public void WithExtraValue_CreatesNoError()
            {
                var processor = CreateProcessor<BaseDriver>("--debug", "--find", "term", "--upload", "this", "value");
                var driver = new SampleDriver();
                processor.Process(driver, Execute);
                processor.Errors.Should().BeEmpty();
            }

            private void Execute(BaseDriver driver, IEnumerable<string> arguments)
            {
                _executedDriver = driver;
                _arguments = arguments.ToList();
            }
        }

        public class WhenHelpRequired : CommandLineProcessorTests
        {
            [Fact]
            public void WhenGivenNullAction_ThrowsException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.WhenHelpRequired(null));
                exception.ParamName.Should().Be("displayAction");
            }

            [Fact]
            public void WhenHelpRequested_ActionIsCalled()
            {
                var called = false;
                var processor = CreateProcessor<BaseDriver>("--help");
                processor.WhenHelpRequired(help => called = true);
                called.Should().BeTrue();
            }

            [Fact]
            public void WhenHelpNotRequested_ActionIsNotCalled()
            {
                var called = false;
                var processor = CreateProcessor<BaseDriver>();
                processor.WhenHelpRequired(help => called = true);
                called.Should().BeFalse();
            }
        }

        public class WhenErrors : CommandLineProcessorTests
        {
            [Fact]
            public void WhenGivenNullAction_ThrowsException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.WhenErrors(null));
                exception.ParamName.Should().Be("displayAction");
            }

            [Fact]
            public void WhenErrorsEncountered_ActionIsCalled()
            {
                var called = false;
                var processor = CreateProcessor<BaseDriver>("--repeat nine");
                processor.Process(new SampleDriver(), Execute)
                    .WhenErrors(errors => called = true);
                called.Should().BeTrue();
            }

            [Fact]
            public void WhenNoErrorsEncountered_ActionIsNotCalled()
            {
                var called = false;
                var processor = CreateProcessor<BaseDriver>("--find", "term");
                processor.Process(new SampleDriver(), Execute)
                    .WhenErrors(errors => called = true);
                processor.WhenErrors(errors => called = true);
                called.Should().BeFalse();
            }

            private void Execute(BaseDriver driver)
            {

            }
        }

    }
}
