using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class CommandLineProcessorTests
    {
        public class Constructor : CommandLineParameterTests
        {
            [Fact]
            public void GivenNullForArguments_ThrowsException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new CommandLineProcessor<BaseDriver>(null, new BaseDriver()));
                exception.ParamName.Should().Be("arguments");
            }

            [Fact]
            public void GivenNullForDriver_ThrowsException()
            {
                var arguments = new List<string> {"--help"};
        public class ProcessWithDriver : CommandLineProcessorTests
        {
            [Fact]
            public void GivenNullDriver_ThrowsExpectedException()
            {
                var processor = CreateProcessor<BaseDriver>();
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => processor.Process(null));
                exception.ParamName.Should().Be("driver");
            }

            [Fact]
            public void WithLongFormSwitch_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("--debug");
                var driver = new BaseDriver();
                processor.Process(driver);
                driver.ShowDiagnostics.Should().BeTrue();
            }

            [Fact]
            public void WithShortFormSwitch_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("-d");
                var driver = new BaseDriver();
                processor.Process(driver);
                driver.ShowDiagnostics.Should().BeTrue();
            }

            [Fact]
            public void WithAlternateShortFormSwitch_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("/d");
                var driver = new BaseDriver();
                processor.Process(driver);
                driver.ShowDiagnostics.Should().BeTrue();
            }

            [Fact]
            public void WithLongFormParameter_CallsMethods()
            {
                var processor = CreateProcessor<BaseDriver>("--find", "file");
                var driver = new SampleDriver();
                processor.Process(driver);
                driver.TextSearch.Should().Be("file");
            }

            [Fact]
            public void WithShortFormParameter_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("-f", "file");
                var driver = new SampleDriver();
                processor.Process(driver);
                driver.TextSearch.Should().Be("file");
            }

            [Fact]
            public void WithAlternateShortFormParameter_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("/f", "file");
                var driver = new SampleDriver();
                processor.Process(driver);
                driver.TextSearch.Should().Be("file");
            }

            [Fact]
            public void WithParameterRequiringConversion_CallsMethod()
            {
                var processor = CreateProcessor<BaseDriver>("/r", "4");
                var driver = new SampleDriver();
                processor.Process(driver);
                driver.Repeats.Should().Be(4);
            }

            [Fact]
            public void WithUnexpectedArgument_LeavesItInArguments()
            {
                var processor = CreateProcessor<BaseDriver>("unexpected");
                var driver = new SampleDriver();
                processor.Process(driver);
                processor.Arguments.Should().Contain("unexpected");
            }

            [Fact]
            public void WithUnexpectedOption_LeavesItInArguments()
            {
                var processor = CreateProcessor<BaseDriver>("--unexpected");
                var driver = new SampleDriver();
                processor.Process(driver);
                processor.Arguments.Should().Contain("--unexpected");
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

        public class OptionHelp : CommandLineParameterTests
        {
            [Fact]
            public void ForValidDriver_ReturnsText()
            {
                var arguments = new List<string> {"test-performance", "--help"};
                var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
                processor.OptionHelp.Should().NotBeEmpty();
            }

            [Fact]
            public void WithNoOptions_ReturnsHelp()
            {
                var processor = new CommandLineProcessor<SampleDriver>(new List<string>(), new SampleDriver());
                processor.OptionHelp.Should().HaveCount(c => c > 0);
            }

            [Fact]
            public void WithNoModes_listsOptions()
            {
                var processor = new CommandLineProcessor<SampleDriver>(new List<string>(), new SampleDriver());
                processor.OptionHelp.Should().HaveCount(6);
            }
        }

        public class Help : CommandLineParameterTests
        {
            [Fact]
            public void ForValidDriver_SetsShowHelp()
            {
                var arguments = new List<string> {"test-performance", "--help"};
                var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
                processor.Help();
                processor.ShowHelp.Should().BeTrue();
            }
        }

        public class Errors : CommandLineParameterTests
        {
            [Fact]
            public void Errors_ForInvalidParameter_ListsContent()
            {
                var arguments = new List<string> {"--not-an-option"};
                var processor = new CommandLineProcessor<BaseDriver>(arguments, new BaseDriver());
                processor.Errors.Should().NotBeEmpty();
            }
        }
    }
}
