namespace Simple.Wpf.Template.Tests
{
    using System;
    using System.Linq;
    using System.Threading;
    using Helpers;
    using NLog;
    using NUnit.Framework;
    using Template;

    [TestFixture]
    public class DurationFixtures
    {
        // Unable to mock out Logger class successfully because method are not marked as virtual,
        // This means have manipulate a real instance of the NLog which involves making test synchonrous to make
        // they don't interfere with each other or I could push the logger behind an interface...
        //
        // Why not push behind an interface? - sick of to many interfaces in projects, and hate the idea of just creating it for testing purposes...
        //
        private static readonly Mutex DurationMutex = new Mutex(false, "Simple.Wpf.Template.Tests.DurationFixtures");
        
        [SetUp]
        public void SetUp()
        {
            DurationMutex.WaitOne();
        }

        [TearDown]
        public void TearDown()
        {
            DurationMutex.ReleaseMutex();
        }

        [Test]
        public void logs_duration_when_debug_level_is_enabled()
        {
            // ARRANGE
            LogHelper.ReconfigureLoggerToLevel(LogLevel.Debug);
            var logger = LogManager.GetCurrentClassLogger();
            var memoryTarget = (LimitedMemoryTarget)LogManager.Configuration.FindTargetByName("memory");
            
            var message = string.Format("Message 1 - {0}", Guid.NewGuid());
            
            // ACT
            using (Duration.Measure(logger, message))
            {
                Thread.Sleep(100);
            }

            LogManager.Flush();

            // ASSERT
            Assert.That(memoryTarget.Logs.Count(x => x.Contains(message)) == 1, Is.True);
        }

        [Test]
        public void does_not_log_duration_when_debug_log_level_is_disabled()
        {
            // ARRANGE
            LogHelper.ReconfigureLoggerToLevel(LogLevel.Info);
            var logger = LogManager.GetCurrentClassLogger();
            var memoryTarget = (LimitedMemoryTarget)LogManager.Configuration.FindTargetByName("memory");

            var message = string.Format("Message 1 - {0}", Guid.NewGuid());

            // ACT
            using (Duration.Measure(logger, message))
            {
                Thread.Sleep(100);
            }

            LogManager.Flush();

            // ASSERT
            Assert.That(memoryTarget.Logs.Count(x => x.Contains(message)) == 1, Is.False);
        }
    }
}
