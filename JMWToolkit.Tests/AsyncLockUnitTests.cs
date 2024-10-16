/*
 * Copyright (c) 2023, jmwileydev@gmail.com
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/
using static JMWToolkit.Tests.MutexHelperUnitTests;

namespace JMWToolkit.Tests;

[TestClass]
public class AsyncLockUnitTests
{
    private readonly static TimeSpan smallDelta = TimeSpan.FromMilliseconds(100);

    public class TryAsyncLockData(AsyncLock asyncLock)
    {
        public AsyncLock AsyncLock { get; private set; } = asyncLock;
        public bool ObtainedLock { get; set; } = false;
        public ManualResetEvent DoneEvent { get; } = new(false);
    }

    public static void TryAsyncLock(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        TryAsyncLockData data = (TryAsyncLockData)obj;
        data.ObtainedLock = data.AsyncLock.Wait(new TimeSpan(0));
        data.DoneEvent.Set();
    }

    public static void ReleaseAsyncLock(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        TryAsyncLockData data = (TryAsyncLockData)obj;
        data.AsyncLock.Release();
        data.DoneEvent.Set();
    }

    [TestMethod]
    [Timeout(10000)]
    public void VerifyAsyncLockWorksOnSingleThread()
    {
        AsyncLock asyncLock = new();

        // Take the lock
        asyncLock.Wait();

        TryAsyncLockData data = new(asyncLock);

        var newThread = new Thread(AsyncLockUnitTests.TryAsyncLock);
        newThread.Start(data);
        data.DoneEvent.WaitOne();
        Assert.IsFalse(data.ObtainedLock);
        asyncLock.Release();
    }

    [TestMethod]
    [Timeout(10000)]
    public void VerifyAsyncLockWorksCanBeReleasedOnAnotherThread()
    {
        AsyncLock asyncLock = new();

        // Take the lock
        asyncLock.Wait();

        TryAsyncLockData data = new(asyncLock);

        var newThread = new Thread(AsyncLockUnitTests.ReleaseAsyncLock);
        newThread.Start(data);
        data.DoneEvent.WaitOne();

        // Ok so far. Make sure we can now lock it on another thread
        data.DoneEvent.Reset();
        newThread = new Thread(AsyncLockUnitTests.TryAsyncLock);
        newThread.Start(data);
        data.DoneEvent.WaitOne();
        Assert.IsTrue(data.ObtainedLock);
        asyncLock.Release();
    }

    [TestMethod]
    [Timeout(10000)]
    public void VerifyAsyncLockWaitWithTimeSpanZeroReturnsImmediately()
    {
        AsyncLock asyncLock = new();

        var startTime = DateTime.Now;
        var locked = asyncLock.Wait(TimeSpan.Zero);
        var endTime = DateTime.Now;
        Assert.IsTrue(endTime - startTime < smallDelta);
        Assert.IsTrue(locked);

        startTime = DateTime.Now;
        locked = asyncLock.Wait(TimeSpan.Zero);
        endTime = DateTime.Now;
        Assert.IsTrue(endTime - startTime < smallDelta);
        Assert.IsFalse(locked);
    }

    [TestMethod]
    [Timeout(10000)]
    public void VerifyAsyncLockWaitWithTimeSpanReturnsWithinDelta()
    {
        AsyncLock asyncLock = new();
        asyncLock.Wait();

        var twoSeconds = TimeSpan.FromSeconds(2);
        var startTime = DateTime.Now;
        var locked = asyncLock.Wait(twoSeconds);
        var endTime = DateTime.Now;
        var delta = twoSeconds - (endTime - startTime);
        Assert.IsTrue(Math.Abs(delta.TotalMicroseconds) < smallDelta.TotalMicroseconds);
        Assert.IsFalse(locked);
    }
}
