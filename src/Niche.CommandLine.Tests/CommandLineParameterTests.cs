using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niche.CommandLine.Tests
{
    [TestFixture]
    public class CommandLineParameterTests
    {
        [Test]
        public void Constructor_missingInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineParameter<string>(null, method);
                });
        }

        [Test]
        public void Constructor_missingMethod_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    new CommandLineParameter<string>(driver, null);
                });
        }

        [Test]
        public void Constructor_ifMethodDoesNotApplyToInstance_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            Assert.Throws<ArgumentException>(
                () =>
                {
                    new CommandLineParameter<string>(this, method);
                });
        }

        [Test]
        public void TryActivate_givenNull_throwsException()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            Assert.Throws<ArgumentNullException>(
            () => commandLineParameter.TryActivate(null));
        }

        [Test]
        public void ConfigureParameters_givenNullInstance_throwsException()
        {
            Assert.Throws<ArgumentNullException>(
            () => CommandLineOptionFactory.CreateParameters(null));
        }

        [Test]
        public void CreateHelp_givenList_AddsEntry()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var help = commandLineParameter.CreateHelp().ToList();
            Assert.That(help, Has.Count.EqualTo(1));
        }

        [Test]
        public void Completed_withNoErrorList_throwsException()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            Assert.Throws<ArgumentNullException>(
                () => commandLineParameter.Completed(null));
        }

        [Test]
        public void Completed_whenRequiredParameterOmitted_generatesError()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            Assert.That(errors, Is.Not.Empty);
        }

        [Test]
        public void Completed_whenRequiredParameterSupplied_generatesNoErrors()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Find");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var arguments = new Queue<string>();
            arguments.Enqueue("-f"); 
            arguments.Enqueue("search");
            commandLineParameter.TryActivate(arguments);
            var errors = new List<string>();
            commandLineParameter.Completed(errors);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void Completed_whenOptionalParameterOmitted_generatesNoErrors()
        {
            var driver = new SampleDriver();
            var findMethod = typeof(SampleDriver).GetMethod("Upload");
            var commandLineParameter = new CommandLineParameter<string>(driver, findMethod);
            var errors = new List<string>();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void Completed_whenSingleValuedParameterProvided_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var arguments = new Queue<string>();
            arguments.Enqueue("-f");
            arguments.Enqueue("search");
            var errors = new List<string>();

            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.Completed(errors);
            Assert.That(driver.TextSearch, Is.EqualTo("search"));
        }

        [Test]
        public void Completed_whenSingleValuedParameterProvidedTwice_createsError()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Find");
            var arguments = new Queue<string>();
            arguments.Enqueue("-f"); 
            arguments.Enqueue("alpha");
            arguments.Enqueue("-f"); 
            arguments.Enqueue("beta");
            var errors = new List<string>();

            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.Completed(errors);
            Assert.That(errors, Is.Not.Empty);
       }

        [Test]
        public void Completed_whenMultiValuedParameterProvided_callsMethod()
        {
            var driver = new SampleDriver();
            var method = driver.GetType().GetMethod("Upload");
            var arguments = new Queue<string>();
            arguments.Enqueue("--upload");
            arguments.Enqueue("alpha");
            arguments.Enqueue("-u"); 
            arguments.Enqueue("beta");
            arguments.Enqueue("/u");
            arguments.Enqueue("gamma");
            var errors = new List<string>();

            var commandLineParameter = new CommandLineParameter<string>(driver, method);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.TryActivate(arguments);
            commandLineParameter.Completed(errors);

            Assert.That(driver.FilesToUpload, Is.EquivalentTo(new List<string> { "alpha", "beta", "gamma" }));
        }
    }
}
