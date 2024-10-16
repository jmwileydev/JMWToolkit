/*
 * Copyright (c) 2023, J.M. Wiley
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

    /// <summary>
    /// Initialize the MutexHelper with the current mutex.
    /// </summary>
    /// <param name="mutex">The mutex to be managed.</param>
    /// <param name="wait">Waits for the mutex to be locked.</param>
    public MutexHelper(Mutex mutex, bool wait = true)
    {
        _mutex = mutex;

        if (wait)
        {
            _ownsMutex = Wait();
        }
    }

    /// <summary>
    /// Waits for the Mutex to be acquired.
    /// </summary>
    /// <returns>true if the mutex is held and false if not.</returns>
    /// <exception cref="InvalidOperationException">The mutex is already owned by this helper.</exception>
    public bool Wait()
    {
        if (_ownsMutex)
        {
            throw new InvalidOperationException("MutexHelper already owns the mutex");
        }

        _ownsMutex = _mutex.WaitOne();
        
        return _ownsMutex;
    }

    /// <summary>
    /// Waits the given amount of time for the mutex to be acquired.
    /// </summary>
    /// <param name="timeout">The amount of time to wait for the mutex.</param>
    /// <returns>true if the lock is acquired, false if not.</returns>
    /// <exception cref="InvalidOperationException">This helper already owns the mutex.</exception>
    public bool Wait(TimeSpan timeout)
    {
        if (_ownsMutex)
        {
            throw new InvalidOperationException("MutexHelper already owns the mutex");
        }

        _ownsMutex = _mutex.WaitOne(timeout);

        return _ownsMutex; ;
    }

    /// <summary>
    /// Releases the mutex.
    /// </summary>
    /// <exception cref="InvalidOperationException">This MutexHelper does not own the mutex.</exception>
    public void Release()
    {
        if (!_ownsMutex)
        {
            throw new InvalidOperationException("MutexHelper already owns the mutex");
        }

        _ownsMutex = false;
        _mutex.ReleaseMutex();
    }

    /// <summary>
    /// Releases the lock if held.
    /// </summary>
    /// <param name="disposing"></param>
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

    /// <summary>
    /// Releases the lock if held.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
