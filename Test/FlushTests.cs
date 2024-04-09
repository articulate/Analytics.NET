using NUnit.Framework;
using Segment.Model;
using System;
using System.Linq;
using System.Threading;

namespace Segment.Test
{
    [TestFixture]
    public class FlushTests
    {
        [SetUp]
        public void Init()
        {
            Analytics.Dispose();
            Logger.Handlers += LoggingHandler;
        }

        [Test]
        public void SynchronousFlushTest()
        {
            Analytics.Initialize(Constants.WRITE_KEY, new Config().SetAsync(false));
            Analytics.Client.Succeeded += Client_Succeeded;
            Analytics.Client.Failed += Client_Failed;

            var trials = 10;

            RunTests(Analytics.Client, trials);

            Assert.That(trials, Is.EqualTo(Analytics.Client.Statistics.Submitted));
            Assert.That(trials, Is.EqualTo(Analytics.Client.Statistics.Succeeded));
            Assert.That(Analytics.Client.Statistics.Failed, Is.EqualTo(0));
        }

        [Test]
        public void AsynchronousFlushTest()
        {
            Analytics.Initialize(Constants.WRITE_KEY, new Config().SetAsync(true));

            Analytics.Client.Succeeded += Client_Succeeded;
            Analytics.Client.Failed += Client_Failed;

            var trials = 10;

            RunTests(Analytics.Client, trials);

            Thread.Sleep(1000); // cant use flush to wait during asynchronous flushing

            Assert.That(trials, Is.EqualTo(Analytics.Client.Statistics.Submitted));
            Assert.That(trials, Is.EqualTo(Analytics.Client.Statistics.Succeeded));
            Assert.That(Analytics.Client.Statistics.Failed, Is.EqualTo(0));
        }

        [Test]
        public void PerformanceTest()
        {
            Analytics.Initialize(Constants.WRITE_KEY);

            Analytics.Client.Succeeded += Client_Succeeded;
            Analytics.Client.Failed += Client_Failed;

            var trials = 100;

            DateTime start = DateTime.Now;

            RunTests(Analytics.Client, trials);

            Analytics.Client.Flush();

            TimeSpan duration = DateTime.Now.Subtract(start);

            Assert.That(trials, Is.EqualTo(Analytics.Client.Statistics.Submitted));
            Assert.That(trials, Is.EqualTo(Analytics.Client.Statistics.Succeeded));
            Assert.That(Analytics.Client.Statistics.Failed, Is.EqualTo(0));

            Assert.That(duration.CompareTo(TimeSpan.FromSeconds(20)), Is.LessThan(0));
        }

        private static void RunTests(Client client, int trials)
        {
            for (var i = 0; i < trials; i += 1)
            {
                Actions.Random(client);
            }
        }

        private static void Client_Failed(BaseAction action, System.Exception e)
        {
            Console.WriteLine($"Action [{action.MessageId}] {action.Type} failed : {e.Message}");
        }

        private static void Client_Succeeded(BaseAction action)
        {
            Console.WriteLine($"Action [{action.MessageId}] {action.Type} succeeded.");
        }

        private static void LoggingHandler(Logger.Level level, string message, Dict args)
        {
            if (args != null)
            {
                message = args.Keys.Aggregate(message, (current, key) => current + $" {"" + key}: {"" + args[key]},");
            }

            Console.WriteLine($"[FlushTests] [{level}] {message}");
        }
    }
}

