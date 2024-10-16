/*
 * Copyright (c) 2023, J.M. Wiley
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/
using System;
using System.Threading.Tasks;

namespace JMWToolkit;

/// <summary>
/// Helper class to manage the hold on an AsyncLock and make sure it gets released properly.
/// </summary>
public class AsyncLockHelper : IDisposable
{
    private bool disposedValue;
    private readonly AsyncLock _asyncLock;
    private bool _ownsLock = false;

    /// <summary>
    /// Initializes the AsyncLockHelper.
    /// </summary>
    /// <param name="asyncLock">The AsyncLock to be managed.</param>
    /// <param name="wait">Whether or not the lock needs to be acquired.</param>
    public AsyncLockHelper(AsyncLock asyncLock, bool wait = true)
    {
        _asyncLock = asyncLock;

        if (wait)
        {
            _ownsLock = _asyncLock.Wait();
        }
    }

    /// <summary>
    /// Asynchronous routine to create an AsyncLockHelper.
    /// </summary>
    /// <param name="asyncLock">The AsyncLock to be managed.</param>
    /// <returns>The AsyncLockHelper with the AsyncLock locked.</returns>
    public static async Task<AsyncLockHelper> CreateAsyncLockHelperAsync(AsyncLock asyncLock)
    {
        var helper = new AsyncLockHelper(asyncLock, false);
        await Task<AsyncLockHelper>.Run(() =>
        {
            helper.Wait();
        });

        return helper;
    }

    /// <summary>
    /// Blocks the current thread until the lock is acquired.
    /// </summary>
    /// <returns>true if the lock was acquired. False otherwise.</returns>
    /// <exception cref="InvalidOperationException">The lock is already locked by this helper.</exception>
    public bool Wait()
    {
        if (_ownsLock)
        {
            throw new InvalidOperationException("Wait cannot be called when the helper is already holding the lock");
        }

        _ownsLock = _asyncLock.Wait();
        return _ownsLock;
    }

    /// <summary>
    /// Blocks the current thread until the lock is acquired, or the timeout is exceeded.
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool Wait(TimeSpan timeout)
    {
        if (_ownsLock)
        {
            throw new InvalidOperationException("Wait cannot be called when the helper is already holding the lock");
        }

        _ownsLock = _asyncLock.Wait(timeout);
        return _ownsLock;
    }

    /// <summary>
    /// Releases the lock.
    /// </summary>
    /// <exception cref="InvalidOperationException">The lock was not acquired by this helper.</exception>
    public void Release()
    {
        if (!_ownsLock)
        {
            throw new InvalidOperationException("Release cannot be called if the lock is not owned.");
        }

        _ownsLock = false;
        _asyncLock.Release();
    }

    /// <summary>
    /// Releases the AsyncLock if it is owned.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing && _ownsLock)
            {
                _asyncLock.Release();
                _ownsLock = false;
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Releases the AsyncLock if it is owned.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
