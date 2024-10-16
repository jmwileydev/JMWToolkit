/*
 * Copyright (c) 2023, jmwileydev@gmail.com
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/

namespace JMWToolkit.Tests;

[TestClass]
public class AsyncLockHelperUnitTests
{
    public class AsyncLockData(AsyncLock asyncLock)
    {
        public AsyncLock AsyncLock { get; private set; } = asyncLock;
        public bool CanLock { get; set; } = false;
        public Exception? CaughtException { get; set; } = null;
        public ManualResetEvent DoneEvent { get; } = new(false);
    }

    public static void TryAsyncLock(object? param)
    {
        ArgumentNullException.ThrowIfNull(param);

        var data = (AsyncLockData)param;

        try
        {
            data.CanLock = data.AsyncLock!.Wait(TimeSpan.Zero);
            if (data.CanLock)
            {
                data.AsyncLock.Release();
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
        AsyncLock testLock = new();
        {
            using AsyncLockHelper helper = new(testLock);
            Console.WriteLine("Testing to make sure AsyncLock is locked");
            {
                AsyncLockData data = new(testLock);

                var newThread = new Thread(AsyncLockHelperUnitTests.TryAsyncLock);
                newThread.Start(data);
                data.DoneEvent.WaitOne();
                Assert.IsFalse(data.CanLock);
            }

            Console.WriteLine("Testing to make sure another Wait on the helper throws an exception");
            Assert.ThrowsException<InvalidOperationException>(() => helper.Wait());
            Assert.ThrowsException<InvalidOperationException>(() => helper.Wait(new TimeSpan(0, 0, 5)));
        }

        // Verify I can lock the AsyncLock
        Assert.IsTrue(testLock.Wait(TimeSpan.Zero));
        testLock.Release();
    }

    [TestMethod]
    [Timeout(5000)]
    public void VerifyDelayedWaitAndReleaseWorkAsExpected()
    {
        AsyncLock testLock = new();

        using (AsyncLockHelper helper = new(testLock, false))
        {
            Assert.IsTrue(testLock.Wait(TimeSpan.Zero));
            testLock.Release();

            Assert.IsTrue(helper.Wait());

            AsyncLockData data = new(testLock);
            var newThread = new Thread(AsyncLockHelperUnitTests.TryAsyncLock);
            newThread.Start(data);
            data.DoneEvent.WaitOne();
            Assert.IsFalse(data.CanLock);
        }

        // Verify I can lock the AsyncLock
        Assert.IsTrue(testLock.Wait(TimeSpan.Zero));
        testLock.Release();
    }
}