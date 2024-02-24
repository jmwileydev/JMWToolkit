using System;
using System.Threading.Tasks;

namespace JMWToolkit;

public class AsyncLockHelper : IDisposable
{
    private bool disposedValue;
    private readonly AsyncLock _asyncLock;
    private bool _ownsLock = false;


    public AsyncLockHelper(AsyncLock asyncLock, bool wait = true)
    {
        _asyncLock = asyncLock;

        if (wait)
        {
            _ownsLock = _asyncLock.Wait();
        }
    }

    public static async Task<AsyncLockHelper> CreateAsyncLockHelperAsync(AsyncLock asyncLock)
    {
        var helper = new AsyncLockHelper(asyncLock, false);
        await Task<AsyncLockHelper>.Run(() =>
        {
            helper.Wait();
        });

        return helper;
    }

    public bool Wait()
    {
        if (_ownsLock)
        {
            throw new InvalidOperationException("Wait cannot be called when the helper is already holding the lock");
        }

        _ownsLock = _asyncLock.Wait();
        return _ownsLock;
    }

    public bool Wait(TimeSpan timeout)
    {
        if (_ownsLock)
        {
            throw new InvalidOperationException("Wait cannot be called when the helper is already holding the lock");
        }

        _ownsLock = _asyncLock.Wait(timeout);
        return _ownsLock;
    }

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
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
