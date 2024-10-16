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
/// Helper class for locking and releasing a SemaphoreSlim object.
/// </summary>
public class SemaphoreSlimHelper : IDisposable
{
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed = false;

    /// <summary>
    /// Initializes the SemaphoreSlimHelper.
    /// </summary>
    /// <param name="semaphore">The Semaphore to be managed.</param>
    public SemaphoreSlimHelper(SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
        _semaphore.Wait();
    }

    /// <summary>
    /// Releases the semaphore if owned.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the semaphore if held.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing && _semaphore != null)
            {
                _semaphore.Release();
            }

            _disposed = true;
        }
    }
}
