using System;
using System.Diagnostics;
using System.Threading;

namespace JMWToolkit;

/// <summary>
/// This class can be used as an AsyncLock. The caller calls AsyncLock.Wait() and then when they
/// are done they call AsyncLock.Release() to release there hold on the lock. The calls do not
/// need to be made on the same thread.
/// </summary>
public class AsyncLock
{
    private readonly Mutex _mutex = new();
    private readonly AutoResetEvent _event = new(false);
    private bool _lockIsHeld = false;
    private readonly TimeSpan _infiniteWait = new(0, 0, 0, 0, -1);

    public AsyncLock(bool initiallyLocked = false)
    {
        if (initiallyLocked)
        {
            _lockIsHeld = true;
        }
    }

    public bool Wait()
    {
        bool lockObtained = false;
        using MutexHelper helper = new(_mutex, false);

        while (!lockObtained)
        {
            // Try and get the mutex, if we cannot then go back to waiting for it to
            // be released.
            if (helper.Wait(TimeSpan.Zero))
            {
                if (!_lockIsHeld)
                {
                    _lockIsHeld = true;
                    lockObtained = true;
                    break;
                }

                helper.Release();
            }

            // If we get here then the mutex is held and we need to wait for it to be
            // released.
            _ = _event.WaitOne();
        }
        return lockObtained;
    }

    public bool Wait(TimeSpan timeout)
    {
        // If it is an infinite wait just call our Wait() routine.
        if (timeout == _infiniteWait)
        {
            return Wait();
        }

        if (timeout < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout),
                "Negative timeout's are not allowed as a Wait time, unless it is -1 which implies Infinite");
        }

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        using MutexHelper helper = new(_mutex, false);

        do
        {
            if (helper.Wait(TimeSpan.Zero))
            {
                if (!_lockIsHeld)
                {
                    _lockIsHeld = true;
                    return true;
                }
            }

            helper.Release();

            timeout -= stopWatch.Elapsed;
            if (timeout > TimeSpan.Zero)
            {
                if (!_event.WaitOne(timeout))
                {
                    return false;
                }

                timeout -= stopWatch.Elapsed;
            }

        } while (timeout > TimeSpan.Zero);

        return false;
    }

    public void Release()
    {
        if (!_lockIsHeld)
        {
            throw new InvalidOperationException("AsyncLock.Release = The lock is not currently being held");
        }

        {
            using var mutexHelper = new MutexHelper(_mutex);
            _lockIsHeld = false;
        }

        // After we release the lock we can signal all the other waiters so someone can get the lock
        _event.Set();
    }
}
