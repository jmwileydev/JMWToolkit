/*
 * Copyright (c) 2023, jmwileydev@gmail.com
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/
using System;
using System.ServiceModel.Channels;
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

    public bool Wait()
    {
        bool lockObtained = false;

        using MutexHelper helper = new(_mutex, false);

        while (!lockObtained)
        {
            helper.Wait();
            {
                if (!_lockIsHeld)
                {
                    _lockIsHeld = true;
                    lockObtained = true;
                    break;
                }
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

        var startTime = DateTime.Now;
        using MutexHelper mutexHelper = new(_mutex, false);
        do
        {
            bool ownsMutex = false;

            ownsMutex = mutexHelper.Wait(timeout);
            if (!ownsMutex)
            {
                return false;
            }

            if (!_lockIsHeld)
            {
                _lockIsHeld = true;
                return true;
            }

            timeout -= (DateTime.Now - startTime);
            if (timeout > TimeSpan.Zero)
            {
                if (!_event.WaitOne(timeout))
                {
                    return false;
                }

                timeout -= (DateTime.Now - startTime);
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

        using (MutexHelper helper = new(_mutex))
        {
            _lockIsHeld = false;
        }

        // After we release the lock we can signal any other waiters
        _event.Set();
    }
}
