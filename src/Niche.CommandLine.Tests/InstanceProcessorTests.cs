using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Niche.CommandLine.Tests
{
    public class InstanceProcessorTests
    {
        private readonly SampleDriver _driver = new SampleDriver();

        private readonly InstanceProcessor<SampleDriver> _processor;

        public InstanceProcessorTests()
        {
            _processor = new InstanceProcessor<SampleDriver>(_driver);
        }

        public class Constructor : InstanceProcessorTests
        {
            [Fact]
            public void GivenNullInstance_ThrowsArgumentNullException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => new InstanceProcessor<SampleDriver>(null));
                exception.ParamName.Should().Be("instance");
            }

            [Fact]
            public void GivenInstance_InitializesInstanceProperty()
            {
                _processor.Instance.Should().Be(_driver);
            }

            [Fact]
            public void GivenInstance_InitializesModesProperty()
            {
                _processor.Modes.Should().HaveCount(1);
            }
            
            [Fact]
            public void GivenInstance_InitializesSwitchesProperty()
            {
                _processor.Switches.Should().HaveCount(1);
            }

            [Fact]
            public void GivenInstance_InitializesParametersProperty()
            {
                _processor.Parameters.Should().HaveCount(3);
            }
        }

        public class Parse : InstanceProcessorTests
        {
            private readonly Queue<string> _arguments = new Queue<string>();

            private readonly List<string> _errors = new List<string>();

            public Parse()
            {
                _arguments.Enqueue("alpha");
                _arguments.Enqueue("beta");
                _arguments.Enqueue("--find");
                _arguments.Enqueue("term");
                _arguments.Enqueue("gamma");
            }

            [Fact]
            public void GivenNullArguments_ThrowsArgumentNullException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _processor.Parse(null, _errors));
                exception.ParamName.Should().Be("arguments");
            }

            [Fact]
            public void GivenNullErrors_ThrowsArgumentNullException()
            {
                var exception =
                    Assert.Throws<ArgumentNullException>(
                        () => _processor.Parse(_arguments, null));
                exception.ParamName.Should().Be("errors");
            }

            [Fact]
            public void WhenArgumentsMatchDriver_ReturnsFewerArguments()
            {
                var count = _arguments.Count;
                _processor.Parse(_arguments, _errors);
                _arguments.Count().Should().BeLessThan(count);
            }

            [Fact]
            public void WhenArgumentsMatchDriver_ConfiguresDriver()
            {
                _processor.Parse(_arguments, _errors);
                _driver.TextSearch.Should().Be("term");
            }
        }
    }
}
