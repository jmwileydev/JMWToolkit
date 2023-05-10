/*
 * Copyright (c) 2023, jmwileydev@gmail.com
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/
using System.Linq.Expressions;

namespace JMWToolkit.Tests;

[TestClass]
public class MutexHelperUnitTests
{
    public class TryLockData
    {
        public TryLockData(Mutex mutex)
        {
            Mutex = mutex;
        }

        public Mutex Mutex { get; private set; }
        public bool CanLock { get; set; } = false;
        public Exception? CaughtException { get; set; } = null;
        public ManualResetEvent DoneEvent { get; } =  new(false);
    }

    public static void TryLockMutex(object? param)
    {
        if (param == null)
        {
            throw new ArgumentNullException(nameof(param));
        }

        var data = (TryLockData)param;

        try
        {
            data.CanLock = data.Mutex!.WaitOne(0);
            if (data.CanLock)
            {
                data.Mutex.ReleaseMutex();
            }

            data.DoneEvent.Set();
        }
        catch (Exception ex)
        {
            data.CaughtException = ex;
            data.DoneEvent.Set();
        }
    }

    [TestMethod]
    [Timeout(5000)]
    public void VerifyConstructorLocksMutexAndDisposeReleasesIt()
    {
        Mutex testMutex = new();
        {
            using MutexHelper helper = new(testMutex);
            Console.WriteLine("Testing to make sure Mutex is locked");
            {
                TryLockData data = new(testMutex);

                var newThread = new Thread(MutexHelperUnitTests.TryLockMutex);
                newThread.Start(data);
                data.DoneEvent.WaitOne();
                Assert.IsFalse(data.CanLock);
            }

            Console.WriteLine("Testing to make sure another Wait on the helper throws an exception");
            Assert.ThrowsException<InvalidOperationException>(() => helper.Wait());
            Assert.ThrowsException<InvalidOperationException>(() => helper.Wait(new TimeSpan(0, 0, 5)));
        }

        // Verify I can lock the mutex
        Assert.IsTrue(testMutex.WaitOne(0));
        testMutex.ReleaseMutex();
    }

    [TestMethod]
    [Timeout(5000)]
    public void VerifyDelayedWaitAndReleaseWorkAsExpected()
    {
        Mutex testMutex = new();

        using (MutexHelper helper = new(testMutex, false))
        {
            Assert.IsTrue(testMutex.WaitOne(0));
            testMutex.ReleaseMutex();

            Assert.IsTrue(helper.Wait());

            TryLockData inner_data = new(testMutex);
            var innerNewThread = new Thread(MutexHelperUnitTests.TryLockMutex);
            innerNewThread.Start(inner_data);
            inner_data.DoneEvent.WaitOne();
            Assert.IsFalse(inner_data.CanLock);
        }

        // Verify I can lock the mutex
        TryLockData data = new(testMutex);
        var newThread = new Thread(MutexHelperUnitTests.TryLockMutex);
        newThread.Start(data);
        data.DoneEvent.WaitOne();
        Assert.IsTrue(data.CanLock);
    }
}