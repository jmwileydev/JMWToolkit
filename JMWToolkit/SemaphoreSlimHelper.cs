/*
 * Copyright (c) 2023, jmwileydev@gmail.com
All rights reserved.

This source code is licensed under the BSD-style license found in the
LICENSE file in the root directory of this source tree. 
*/
using System;
using System.Threading;

namespace JMWToolkit;

public class SemaphoreSlimHelper : IDisposable
{
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed = false;

    public SemaphoreSlimHelper(SemaphoreSlim semaphore)
    {
        _semaphore = semaphore;
        _semaphore.Wait();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
