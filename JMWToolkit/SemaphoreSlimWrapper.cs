using System;
using System.Threading;

namespace JMWToolkit;

public class SemaphoreSlimWrapper : IDisposable
{
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed = false;

    public SemaphoreSlimWrapper(SemaphoreSlim semaphore)
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
