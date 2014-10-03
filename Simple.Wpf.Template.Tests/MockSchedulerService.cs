﻿namespace Simple.Wpf.Template.Tests
{
    using System.Reactive.Concurrency;
    using Microsoft.Reactive.Testing;
    using Services;

    public sealed class MockSchedulerService : ISchedulerService
    {
        private readonly TestScheduler _testScheduler;

        public MockSchedulerService(TestScheduler testScheduler)
        {
            _testScheduler = testScheduler;
        }

        public IScheduler Dispatcher
        {
            get { return _testScheduler; }
        }

        public IScheduler Current
        {
            get { return _testScheduler; }
        }

        public IScheduler TaskPool
        {
            get { return _testScheduler; }
        }

        public IScheduler EventLoop
        {
            get { return _testScheduler; }
        }

        public IScheduler NewThread
        {
            get { return _testScheduler; }
        }
        
        public IScheduler StaThread
        {
            get { return _testScheduler; }
        }
    }
}