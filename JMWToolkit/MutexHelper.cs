/*
 * Copyright (c) 2023, jmwileydev@gmail.com
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/
using System;
using System.Threading;

namespace JMWToolkit;

/// <summary>
/// This class is used to help with making sure ReleaseMutex is called when the scope of
/// the method where the mutex is held exits. This is super convenient for a routine which
/// grabs the mutex, does some work and then releases it:
/// 
/// Mutex _mutex;
/// using (new MutexHelper(_mutex))
/// {
///     // Do some work here that requires the mutex
/// }
/// 
/// ReleaseMutex will be called when the using statement exits.
/// </summary>
public class MutexHelper : IDisposable
{

    private readonly Mutex _mutex;
    private bool _ownsMutex;
    private bool disposedValue = false;

    public MutexHelper(Mutex mutex, bool wait = true)
    {
        _mutex = mutex;

        if (wait)
        {
            _ownsMutex = Wait();
        }
    }

    public MutexHelper(Mutex mutex, TimeSpan timeout)
    {
        _mutex = mutex;
        Wait(timeout);
    }

    public bool Wait()
    {
        if (_ownsMutex)
        {
            throw new InvalidOperationException("MutexHelper already owns the mutex");
        }

        _ownsMutex = _mutex.WaitOne();
        
        return _ownsMutex;
    }

    public bool Wait(TimeSpan timeout)
    {
        if (_ownsMutex)
        {
            throw new InvalidOperationException("MutexHelper already owns the mutex");
        }

        _ownsMutex = _mutex.WaitOne(timeout);

        return _ownsMutex; ;
    }

    public void Release()
    {
        if (!_ownsMutex)
        {
            throw new InvalidOperationException("MutexHelper already owns the mutex");
        }

        _mutex.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing && _ownsMutex)
            {
                _mutex.ReleaseMutex();
                _ownsMutex = false;
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
